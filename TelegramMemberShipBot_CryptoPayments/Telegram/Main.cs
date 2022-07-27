using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using TelegramMemberShipBot_CryptoPayments.Coinpayments;
using TelegramMemberShipBot_CryptoPayments.Models;

namespace TelegramMemberShipBot_CryptoPayments.Telegram
{
    public static class Main
    {
        static CancellationTokenSource CancelToken { get; set; }
        public static TelegramBotClient Bot { get; set; }

        public static void StartBot()
        {
            CancelToken = new CancellationTokenSource();
            Bot = new TelegramBotClient(SettingsManager.TelegramBotToken);

            var receiverOptions = new ReceiverOptions()
            {
                AllowedUpdates = Array.Empty<UpdateType>(),
                ThrowPendingUpdates = true
            };

            Bot.StartReceiving(updateHandler: UpdateHandlers.HandleUpdateAsync,
                               pollingErrorHandler: UpdateHandlers.PollingErrorHandler,
                               receiverOptions: receiverOptions,
                               cancellationToken: CancelToken.Token);

            RunInBackground(TimeSpan.FromMinutes(SettingsManager.UpdatePaymentsAndUsersTickMunite), () => UpdateUsersTick());
        }

        static async Task RunInBackground(TimeSpan timeSpan, Action action)
        {
            var periodicTimer = new PeriodicTimer(timeSpan);
            while (await periodicTimer.WaitForNextTickAsync())
            {
                action();
            }
        }

        static async Task UpdateUsersTick()
        {
			Console.WriteLine("Kontrol başladı.");
			//Ban expired users
			List<Models.User> Expired_Users = new Database().GetExpiredUsers();
			using (List<Models.User>.Enumerator enumerator = Expired_Users.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TelegramBotClientExtensions.BanChatMemberAsync(userId: enumerator.Current.UserId, botClient: Bot, chatId: new ChatId(SettingsManager.TelegramChannelId));
				}
			}

			//Check uncomplated payments
			var Database = new Database();
			List<Payment> Last8HoursPayments = Database.GetPendingPayments();
			for (int i = 0; i < Last8HoursPayments.Count; i += 25)
			{
				//Parse payments to has 25 elemetent lists and call api.
				//After getting response check payments are finished or cancelled.
				//Update database

				//Call api and get response
				int count = 25;
				if (i + 25 > Last8HoursPayments.Count)
				{
					count = Last8HoursPayments.Count - i;
				}
				List<Payment> get_payments_list = Last8HoursPayments.GetRange(i, count);
				ReceiveTransactionsInfoResponse transactions = await Coinpayments.Main.Api.GetTransactionInfoMultiAsync(new ReceiveTransactionsInfo
				{
					TransactionIds = string.Join("|", get_payments_list.Select((Payment x) => x.TransactionId.ToString()))
				});
				int j = 0;

				//Check payments
				while (i < transactions.Result.Count)
				{
					KeyValuePair<string, ReceiveTransactionInfoResult> trans = transactions.Result.ElementAt(j);
					if (trans.Value.Status > 0)
					{
						//Update database
                        Database.ComplatePayment(Last8HoursPayments[i + j]);
						Bot.SendTextMessageAsync(Last8HoursPayments[i + j].UserId, SettingsManager.TelegramPaymentSuccessText);
						if (Last8HoursPayments[i + j].ISCCNews)
						{
							TelegramBotClient bot = Bot;
							ChatId chatId = new(SettingsManager.TelegramChannelId);
							ChatInviteLink invite = await bot.CreateChatInviteLinkAsync(chatId, null, null, 1);

							Bot.UnbanChatMemberAsync(new ChatId(SettingsManager.TelegramChannelId), Last8HoursPayments[i + j].UserId);
							Bot.SendTextMessageAsync(Last8HoursPayments[i + j].UserId, SettingsManager.TelegramCCNewsPaymentSuccessText(invite.InviteLink));
						}
						else
						{
							Bot.SendTextMessageAsync(Last8HoursPayments[i + j].UserId, SettingsManager.TelegramCELBPaymentSuccessText);
						}
					}//else if cancelled
					else if (trans.Value.Status == -1)
                    {
						Database.CancelPayment(Last8HoursPayments[i + j]);
                    }
					j++;
				}
			}
		}
    }
}
