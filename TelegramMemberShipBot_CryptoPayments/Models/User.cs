namespace TelegramMemberShipBot_CryptoPayments.Models
{
    public class User
    {
        public int Id { get; set; }

        public long UserId { get; set; }

        public string? Username { get; set; }

        public string? FullName { get; set; }

        public DateTime EndDate { get; set; }

        public double LastPaymentAmountMain { get; set; }

        public double LastPaymentAmountCoin { get; set; }

        public string LastPaymentCoinName { get; set; }

    }
}
