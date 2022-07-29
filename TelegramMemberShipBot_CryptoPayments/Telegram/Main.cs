using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using TelegramMemberShipBot_CryptoPayments.Coinpayments;
using TelegramMemberShipBot_CryptoPayments.Models;
using TelegramMemberShipBot_CryptoPayments.Database;

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
			Console.WriteLine("| Check started |");
            try
            {
				DatabaseContext DbContext = new();

				//Ban expired users
				List<Models.User> Expired_Users = DbContext.GetExpiredUsers();
				foreach (Models.User ExpiredUser in Expired_Users)
                {
                    try
                    {
						await Bot.UnbanChatMemberAsync(userId: ExpiredUser.UserId, chatId: new ChatId(SettingsManager.TelegramChannelId));
						DbContext.DelUser(ExpiredUser);
						Console.WriteLine($"Banned User : {ExpiredUser.FullName}");
                    }
                    catch (Exception ex)
                    {
						Console.WriteLine($"Error to Ban User : {ex.Message}");
					}

				}

				//Check uncomplated payments
				List<Payment> PendingPayments = DbContext.GetPendingPayments();
				for (int i = 0; i < PendingPayments.Count; i += 25)
				{
					//Parse payments to has 25 elemetent lists and call api.Because api gives per one time max 25 payments info.
					//After getting response check payments are finished or cancelled.
					//Update database

					//Call api and get response
					List<Payment> Payments_List = PendingPayments.GetRange(i, (i + 25 > PendingPayments.Count ? PendingPayments.Count - i : 25)); // if i + 25 > pending_payments.count => get elements from i to last
					ReceiveTransactionsInfoResponse Transactions = await Coinpayments.Main.Api.GetTransactionInfoMultiAsync(new ReceiveTransactionsInfo
					{
						TransactionIds = string.Join("|", Payments_List.Select((Payment x) => x.TransactionId.ToString()))
					});

					//Check payments
					for (int j = 0; j < Transactions.Result.Count; j++)
                    {
						var _Transaction = PendingPayments[i + j];// Transaction from database
						var Transaction = Transactions.Result.ElementAt(j).Value; // Transaction info from api => KeyValuePair<string, ReceiveTransactionInfoResult>().Value

						if (Transaction.StatusText == "Complete") // If transection success
                        {
							DbContext.ComplatePayment(_Transaction, Transaction.StatusText);
							await Bot.SendTextMessageAsync(_Transaction.UserId, SettingsManager.TelegramPaymentSuccessText);

							if (_Transaction.ISCCNews)
							{
                                try
                                {
									ChatId chatId = new(SettingsManager.TelegramChannelId);
									ChatInviteLink invite = await Bot.CreateChatInviteLinkAsync(chatId, null, null, 1);

									await Bot.SendTextMessageAsync(_Transaction.UserId, SettingsManager.TelegramCCNewsPaymentSuccessText(invite.InviteLink));
                                }
                                catch (Exception ex)
                                {
									Console.WriteLine($"Error to Add User : {ex.Message}");
								}
							}
							else
							{
								await Bot.SendTextMessageAsync(_Transaction.UserId, SettingsManager.TelegramCELBPaymentSuccessText);
							}
							Console.WriteLine($"Payment Complated : {_Transaction.FullName} | {_Transaction.TransactionId}");
						}
						else if (Transaction.StatusText == "Cancelled / Timed Out") // If transaction cancelled
						{
							DbContext.CancelPayment(_Transaction);
							Console.WriteLine($"Payment Cancelled : {_Transaction.FullName} | {_Transaction.TransactionId}");
						}
					}
				}
				DbContext.SaveChanges();
				DbContext.Dispose();
			}
            catch (Exception ex)
            {
				Console.WriteLine($"Unexpected Error : {ex.Message}");
            }

			Console.WriteLine("| Check Over |");
		}
	}
}
