namespace TelegramMemberShipBot_CryptoPayments.Models
{
    public class Payment
    {
		public int Id { get; set; }

		public long UserId { get; set; }

		public string? Username { get; set; }

		public string? FullName { get; set; }

		public string TransactionId { get; set; }

		public string PaymentAdress { get; set; }

		public DateTime CreateDate { get; set; }

		public DateTime ExpiryDate { get; set; }

		public DateTime? ComplateDate { get; set; }

		public short LastStatus { get; set; }

		public bool ISCCNews { get; set; }

		public double AmountCoin { get; set; }

		public double AmountMain { get; set; }

		public string Coin { get; set; }
	}
}
