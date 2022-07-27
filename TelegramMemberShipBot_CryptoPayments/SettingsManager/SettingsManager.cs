using Newtonsoft.Json;
using TelegramMemberShipBot_CryptoPayments.Models;

namespace TelegramMemberShipBot_CryptoPayments
{
    public static class SettingsManager
    {
        private static Settings Settings { get; set; }

        public static void LoadSettings()
        {
            try
            {
                using StreamReader r = new("settings.json");
                string json = r.ReadToEnd();
                Settings = JsonConvert.DeserializeObject<Settings>(json);
            }
            catch
            {
                Console.WriteLine("Ayarlar Yüklenemedi !");
                Settings = new Settings();
            }
        }

        public static void SaveSettings()
        {
            try
            {
                using StreamWriter w = File.CreateText("settings.json");
                string json = JsonConvert.SerializeObject(Settings, Formatting.Indented);
                w.Write(json);
            }
            catch
            {
                Console.WriteLine("Ayarlar Kaydedilemedi !");
            }
        }
        
        public static void SetDatabaseEnsureCreated(bool isCreated = true)
        {
            Settings.IsDatabaseEnsureCreated = isCreated;
            SaveSettings();
        }
        
        public static void DatabaseResetted()
        {
            Settings.ResetDatabaseFirstStart = false;
            SetDatabaseEnsureCreated(false);
        }

        public static string DatabaseConnectionString => Settings.DatabaseConnectionString;

        public static bool IsDatabaseEnsureCreated => Settings.IsDatabaseEnsureCreated;

        public static bool ResetDatabaseFirstStart => Settings.ResetDatabaseFirstStart;

        public static string CoinpaymentsApiUrl => Settings.CoinpaymentsApiUrl;

        public static string CoinpaymentsApiPublicKey => Settings.CoinpaymentsApiPublicKey;

        public static string CoinpaymentsApiPrivateKey => Settings.CoinpaymentsApiPrivateKey;
        
        public static string CoinpaymentsBuyerEmail => Settings.CoinpaymentsBuyerEmail;

        public static bool EnableLitecoinTestPayment => Settings.EnableLitecoinTestPayment;

        public static string TelegramBotToken => Settings.TelegramBotToken;

        public static string TelegramChannelId => Settings.TelegramChannelId;
        
        public static double UpdatePaymentsAndUsersTickMunite => Settings.UpdatePaymentsAndUsersTickMunite;


        public static double CcNewsMembershipPrice => Settings.CcNewsMembershipPrice;

        public static string CcNewsMembershipPeriot => Settings.CcNewsMembershipPeriot;

        public static double CCNewsMembershipPeriotDays => Settings.CCNewsMembershipPeriotDays;

        public static double CelbMembershipPrice => Settings.CelbMembershipPrice;

        public static string CelbMembershipPeriot => Settings.CelbMembershipPeriot;

        public static string MainPriceType => Settings.MainPriceType;


        public static string TelegramCcNewsButtonText => Settings.TelegramCcNewsButtonText;

        public static string TelegramCelbButtonText => Settings.TelegramCelbButtonText;

        public static string TelegramCcNewsBotButtonText => Settings.TelegramCcNewsBotButtonText;


        public static string TelegramCcNewsDescriptionText => Settings.TelegramCcNewsDescriptionText;

        public static string TelegramCelbDescriptionText => Settings.TelegramCelbDescriptionText;

        public static string TelegramCcNewsBotDescriptionText => Settings.TelegramCcNewsBotDescriptionText;

        
        public static string TelegramBackButtonText => Settings.TelegramBackButtonText;

        public static string TelegramStatusButtonText => Settings.TelegramStatusButtonText;

        public static string TelegramStatusNotMemberMessage => Settings.TelegramStatusNotMemberMessage;
        
        public static string TelegramStatusMemberMessage(DateTime expiryDate) => Settings.TelegramStatusMemberMessage
            .Replace("*[expirydate]*", expiryDate.ToString());


        public static string TelegramWelcomeText => Settings.TelegramWelcomeText;

        public static string TelegramCcNewsMembershipButtonText => Settings.TelegramCcNewsMembershipButtonText
            .Replace("*[price]*", Settings.CcNewsMembershipPrice.ToString())
            .Replace("*[pricetype]*", Settings.MainPriceType)
            .Replace("*[periot]*", Settings.CcNewsMembershipPeriot);

        public static string TelegramCelbMembershipButtonText => Settings.TelegramCelbMembershipButtonText
            .Replace("*[price]*", Settings.CelbMembershipPrice.ToString())
            .Replace("*[pricetype]*", Settings.MainPriceType)
            .Replace("*[periot]*", Settings.CelbMembershipPeriot);

        public static string TelegramPaymentText(double amount, double price, string coin, string adress, string time, string statuslink) => Settings.TelegramPaymentText
            .Replace("*[amount]*", amount.ToString())
            .Replace("*[price]*", price.ToString())
            .Replace("*[coin]*", coin)
            .Replace("*[adress]*", adress)
            .Replace("*[time]*", time)
            .Replace("*[statuslink]*", statuslink);

        public static string TelegramPaymentSuccessText => Settings.TelegramPaymentSuccessText;

        public static string TelegramCCNewsPaymentSuccessText(string inviteLink) => Settings.TelegramCCNewsPaymentSuccessText
            .Replace("*[inviteLink]*", inviteLink);

        public static string TelegramCELBPaymentSuccessText => Settings.TelegramCELBPaymentSuccessText;
    }
}
