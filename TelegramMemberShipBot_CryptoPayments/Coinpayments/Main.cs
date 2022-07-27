namespace TelegramMemberShipBot_CryptoPayments.Coinpayments
{
    public class Main
    {
        public static CoinApi Api { get; set; }

        public static void Setup_CPM()
        {
            Api = new(SettingsManager.CoinpaymentsApiPublicKey, SettingsManager.CoinpaymentsApiPrivateKey);
        }
    }
}
