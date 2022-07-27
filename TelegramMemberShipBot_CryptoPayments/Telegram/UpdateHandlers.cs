using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramMemberShipBot_CryptoPayments.Models;
using TelegramMemberShipBot_CryptoPayments.Coinpayments;

namespace TelegramMemberShipBot_CryptoPayments.Telegram
{
    public static class UpdateHandlers
    {
        //Main Methods
        public static Task PollingErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => BotOnMessageReceived(botClient, update.Message!),
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(botClient, update.CallbackQuery!)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await PollingErrorHandler(botClient, exception, cancellationToken);
            }
        }

        //New Message Or Callback Handled Methods
        private static Task BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            if (message.Text is not { } messageText)
                return Task.Delay(0);

            if (messageText == "/start") DefaultMessage(botClient, message);
            else if (messageText == SettingsManager.TelegramCcNewsButtonText) CCNewsMessage(botClient, message);
            else if (messageText == SettingsManager.TelegramCelbButtonText) CELBMessage(botClient, message);
            else if (messageText == SettingsManager.TelegramCcNewsBotButtonText) CCNewsBotMessage(botClient, message);

            return Task.Delay(0);
        }

        private static async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            if (callbackQuery.Data == "back_ccnews")
                CCNewsMembershipBackMessage(botClient, callbackQuery.Message);
            else if (callbackQuery.Data == "back_celb")
                CELBMembershipBackMessage(botClient, callbackQuery.Message);
            else if (callbackQuery.Data == "status_ccnews")
                CCNewsStatusMessage(botClient, callbackQuery.Message);
            else if (callbackQuery.Data == "membership_ccnews")
                CCNewsMembershipMessage(botClient, callbackQuery.Message);
            else if (callbackQuery.Data == "membership_celb")
                CELBMembershipMessage(botClient, callbackQuery.Message);
            else if (callbackQuery.Data!.StartsWith("ccnews_") || callbackQuery.Data!.StartsWith("celb_"))
                StartPayment(botClient, callbackQuery);
        }

        //Answer Methods
        static async Task<Message> DefaultMessage(ITelegramBotClient botClient, Message message)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(
                new[] { new KeyboardButton[] { 
                    SettingsManager.TelegramCcNewsButtonText, 
                    SettingsManager.TelegramCelbButtonText, 
                    SettingsManager.TelegramCcNewsBotButtonText } })
            { ResizeKeyboard = true };

            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id, parseMode: ParseMode.Markdown,
                                                        text: SettingsManager.TelegramWelcomeText,
                                                        replyMarkup: replyKeyboardMarkup);
        }
        
        static async Task<Message> CCNewsMessage(ITelegramBotClient botClient, Message message)
        {
            InlineKeyboardMarkup replyKeyboardMarkup = new( new[] { 
                new [] { InlineKeyboardButton.WithCallbackData(SettingsManager.TelegramCcNewsMembershipButtonText, "membership_ccnews") },
                new [] { InlineKeyboardButton.WithCallbackData(SettingsManager.TelegramStatusButtonText, "status_ccnews") } });

            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id, parseMode: ParseMode.Markdown,
                                                        text: SettingsManager.TelegramCcNewsDescriptionText,
                                                        replyMarkup: replyKeyboardMarkup);
        }
        
        static async Task<Message> CELBMessage(ITelegramBotClient botClient, Message message)
        {
            InlineKeyboardMarkup replyKeyboardMarkup = new(new[] {
                new [] { InlineKeyboardButton.WithCallbackData(SettingsManager.TelegramCelbMembershipButtonText, "membership_celb") }  });

            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id, parseMode: ParseMode.Markdown,
                                                        text: SettingsManager.TelegramCelbDescriptionText,
                                                        replyMarkup: replyKeyboardMarkup);
        }
        
        static async Task<Message> CCNewsBotMessage(ITelegramBotClient botClient, Message message)
        {
            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id, parseMode: ParseMode.Markdown,
                                                        text: SettingsManager.TelegramCcNewsBotDescriptionText);
        }

            //Button Click Events Answers
        static async Task<Message> CCNewsMembershipMessage(ITelegramBotClient botClient, Message message)
        {
            InlineKeyboardMarkup keyboardMarkup = new( new[] { 
                new [] { InlineKeyboardButton.WithCallbackData("⚡ Tether USD (Tron/TRC20)", "ccnews_usdt.trc20")   },
                new [] { InlineKeyboardButton.WithCallbackData("⚡ USDC Coin (Tron/TRC20)", "ccnews_usdc.trc20")   },
                new [] { InlineKeyboardButton.WithCallbackData("⚡ BUSD Token (BSC Chain)", "ccnews_busd.bep20") },
                new [] { InlineKeyboardButton.WithCallbackData("⚡ BTC (Bitcoin)", "ccnews_btc")   },
                new [] { InlineKeyboardButton.WithCallbackData("⚡ ETH (Ethereum)", "ccnews_eth")   },
                new [] { InlineKeyboardButton.WithCallbackData("⚡ XRP (XRP Token (BC Chain))", "ccnews_xrp.bep2")   },
                new [] { InlineKeyboardButton.WithCallbackData("⚡ LCT (Litecoin)", "ccnews_ltc")   },
                new [] { InlineKeyboardButton.WithCallbackData("⚡ SOL (Solana)", "ccnews_sol")   },
                new [] { InlineKeyboardButton.WithCallbackData("⚡ TRX (Tron)", "ccnews_trx")   },
                new [] { InlineKeyboardButton.WithCallbackData("⚡ DOGE (Dogecoin)", "ccnews_doge") },
                new [] { InlineKeyboardButton.WithCallbackData("⚡ SHIB (SHIBA INU (ERC20))", "ccnews_shib") },
                new [] { InlineKeyboardButton.WithCallbackData("⚡ XMR (Monero)", "ccnews_xmr")   },
                new [] { InlineKeyboardButton.WithCallbackData("<< Back", "back_ccnews")   } });

            if (SettingsManager.EnableLitecoinTestPayment)
            {
                keyboardMarkup = new InlineKeyboardMarkup(keyboardMarkup.InlineKeyboard.Append(new InlineKeyboardButton[1] { InlineKeyboardButton.WithCallbackData("LTC TEST", "ccnews_ltct") }));
            }

            return await botClient.EditMessageTextAsync(chatId: message.Chat.Id, messageId: message.MessageId, parseMode:ParseMode.Markdown,
                                                        text: SettingsManager.TelegramCcNewsDescriptionText,
                                                        replyMarkup: keyboardMarkup);
        }
        
        static async Task<Message> CELBMembershipMessage(ITelegramBotClient botClient, Message message)
        {
            InlineKeyboardMarkup keyboardMarkup = new( new[] {
                new [] { InlineKeyboardButton.WithCallbackData("⚡ Tether USD (Tron/TRC20)", "celb_usdt.trc20")   },
                new [] { InlineKeyboardButton.WithCallbackData("⚡ USDC Coin (Tron/TRC20)", "celb_usdc.trc20")   },
                new [] { InlineKeyboardButton.WithCallbackData("⚡ BUSD Token (BSC Chain)", "celb_busd.bep20") },
                new [] { InlineKeyboardButton.WithCallbackData("⚡ BTC (Bitcoin)", "celb_btc")   },
                new [] { InlineKeyboardButton.WithCallbackData("⚡ ETH (Ethereum)", "celb_eth")   },
                new [] { InlineKeyboardButton.WithCallbackData("⚡ XRP (XRP Token (BC Chain))", "celb_xrp.bep2")   },
                new [] { InlineKeyboardButton.WithCallbackData("⚡ LCT (Litecoin)", "celb_ltc")   },
                new [] { InlineKeyboardButton.WithCallbackData("⚡ SOL (Solana)", "celb_sol")   },
                new [] { InlineKeyboardButton.WithCallbackData("⚡ TRX (Tron)", "celb_trx")   },
                new [] { InlineKeyboardButton.WithCallbackData("⚡ DOGE (Dogecoin)", "celb_doge") },
                new [] { InlineKeyboardButton.WithCallbackData("⚡ SHIB (SHIBA INU (ERC20))", "celb_shib") },
                new [] { InlineKeyboardButton.WithCallbackData("⚡ XMR (Monero)", "celb_xmr")   },
                new [] { InlineKeyboardButton.WithCallbackData("<< Back", "back_celb")   } });

            if (SettingsManager.EnableLitecoinTestPayment)
            {
                keyboardMarkup = new InlineKeyboardMarkup(keyboardMarkup.InlineKeyboard.Append(new InlineKeyboardButton[1] { InlineKeyboardButton.WithCallbackData("LTC TEST", "celb_ltct") }));
            }

            return await botClient.EditMessageTextAsync(chatId: message.Chat.Id, messageId: message.MessageId, parseMode:ParseMode.Markdown,
                                                        text: SettingsManager.TelegramCelbDescriptionText,
                                                        replyMarkup: keyboardMarkup);
        }
        
        static async Task<Message> CCNewsStatusMessage(ITelegramBotClient botClient, Message message)
        {
            var user = new Database().GetCCNewsStatus(message.Chat.Id);
            string? sendMessage = user.UserId == -1 ? SettingsManager.TelegramStatusNotMemberMessage : SettingsManager.TelegramStatusMemberMessage(user.EndDate);

            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id, parseMode: ParseMode.Markdown,
                                                        text: sendMessage);
        }
        
                //Answered Messages Buttons Click Events Answers
        static async Task<Message> CCNewsMembershipBackMessage(ITelegramBotClient botClient, Message message)
        {
            InlineKeyboardMarkup replyKeyboardMarkup = new(
                new[] { new InlineKeyboardButton[] { SettingsManager.TelegramCcNewsMembershipButtonText },
                        new InlineKeyboardButton[] { SettingsManager.TelegramStatusButtonText } });

            return await botClient.EditMessageTextAsync(chatId: message.Chat.Id, messageId: message.MessageId, parseMode: ParseMode.Markdown,
                                                        text: SettingsManager.TelegramCcNewsDescriptionText,
                                                        replyMarkup: replyKeyboardMarkup);
        }
        
        static async Task<Message> CELBMembershipBackMessage(ITelegramBotClient botClient, Message message)
        {
            InlineKeyboardMarkup replyKeyboardMarkup = new(
                new[] { new InlineKeyboardButton[] { SettingsManager.TelegramCelbMembershipButtonText } });

            return await botClient.EditMessageTextAsync(chatId: message.Chat.Id, messageId: message.MessageId, parseMode: ParseMode.Markdown,
                                                        text: SettingsManager.TelegramCelbDescriptionText,
                                                        replyMarkup: replyKeyboardMarkup);
        }

        //Clicked Payment Buttons Method
        static Task StartPayment(ITelegramBotClient botClient, CallbackQuery query)
        {
            Task.Run(async () => 
            {
                var coinName = query.Data.Split("_")[1].ToUpper();
                var price = query.Data.StartsWith("ccnews_") ? SettingsManager.CcNewsMembershipPrice : SettingsManager.CelbMembershipPrice;

                var trans = await Coinpayments.Main.Api.CreateTransactionAsync(new ReceiveTransaction() 
                { 
                    Amount=price, 
                    BuyerEmail=SettingsManager.CoinpaymentsBuyerEmail, 
                    Currency1= SettingsManager.MainPriceType, 
                    Currency2 = coinName 
                });

                var pay = new Payment() 
                {   
                    TransactionId = trans.Result.TransactionId, 
                    AmountCoin = trans.Result.Amount, 
                    AmountMain = price, 
                    Coin = coinName, 
                    CreateDate = DateTime.Now, 
                    ExpiryDate = DateTime.UnixEpoch.AddSeconds(trans.Result.Timeout),
                    ComplateDate = null,
                    LastStatus = 0,
                    ISCCNews = query.Data.StartsWith("ccnews_"),
                    PaymentAdress = trans.Result.Address, 
                    UserId = query.Message.Chat.Id, 
                    Username = query.Message.Chat.Username, 
                    FullName = query.Message.Chat.FirstName + " " + query.Message.Chat.LastName
                };

                new Database().AddPayment(pay);

                //Send message and qr
                await botClient.SendTextMessageAsync(chatId: query.Message.Chat.Id, parseMode: ParseMode.Markdown,
                                                            text: SettingsManager.TelegramPaymentText(trans.Result.Amount, price, coinName, trans.Result.Address, (trans.Result.Timeout / 3600).ToString(), trans.Result.StatusUrl));

                await botClient.SendPhotoAsync(chatId: query.Message.Chat.Id, parseMode: ParseMode.Markdown,
                                                            photo: trans.Result.QrCodeUrl);
            });
            return Task.Delay(0);
        }
    }
}