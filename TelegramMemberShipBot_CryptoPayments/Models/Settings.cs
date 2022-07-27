using Newtonsoft.Json;

namespace TelegramMemberShipBot_CryptoPayments.Models
{
    public class Settings
    {
		[JsonProperty("DatabaseConnectionString")]
		public string DatabaseConnectionString { get; set; }

		[JsonProperty("IsDatabaseEnsureCreated")]
		public bool IsDatabaseEnsureCreated { get; set; }
		
		[JsonProperty("ResetDatabaseFirstStart")]
		public bool ResetDatabaseFirstStart { get; set; }

		[JsonProperty("CoinpaymentsApiUrl")]
		public string CoinpaymentsApiUrl { get; set; }

		[JsonProperty("CoinpaymentsApiPublicKey")]
		public string CoinpaymentsApiPublicKey { get; set; }

		[JsonProperty("CoinpaymentsApiPrivateKey")]
		public string CoinpaymentsApiPrivateKey { get; set; }

		[JsonProperty("CoinpaymentsBuyerEmail")]
		public string CoinpaymentsBuyerEmail { get; set; }


		[JsonProperty("EnableLitecoinTestPayment")]
		public bool EnableLitecoinTestPayment { get; set; }

		[JsonProperty("TelegramBotToken")]
		public string TelegramBotToken { get; set; }

		[JsonProperty("TelegramChannelId")]
		public string TelegramChannelId { get; set; }
		
		[JsonProperty("UpdatePaymentsAndUsersTickMunite")]
		public double UpdatePaymentsAndUsersTickMunite { get; set; }


		[JsonProperty("MainPriceType")]
		public string MainPriceType { get; set; }

		[JsonProperty("CCNewsMembershipPrice")]
		public long CcNewsMembershipPrice { get; set; }

		[JsonProperty("CCNewsMembershipPeriot")]
		public string CcNewsMembershipPeriot { get; set; }
		
		[JsonProperty("CCNewsMembershipPeriotDays")]
		public double CCNewsMembershipPeriotDays { get; set; }

		[JsonProperty("CELBMembershipPrice")]
		public long CelbMembershipPrice { get; set; }

		[JsonProperty("CELBMembershipPeriot")]
		public string CelbMembershipPeriot { get; set; }


		[JsonProperty("TelegramCCNewsButtonText")]
		public string TelegramCcNewsButtonText { get; set; }

		[JsonProperty("TelegramCELBButtonText")]
		public string TelegramCelbButtonText { get; set; }

		[JsonProperty("TelegramCCNewsBotButtonText")]
		public string TelegramCcNewsBotButtonText { get; set; }


		[JsonProperty("TelegramCCNewsDescriptionText")]
		public string TelegramCcNewsDescriptionText { get; set; }

		[JsonProperty("TelegramCELBDescriptionText")]
		public string TelegramCelbDescriptionText { get; set; }

		[JsonProperty("TelegramCCNewsBotDescriptionText")]
		public string TelegramCcNewsBotDescriptionText { get; set; }


		[JsonProperty("TelegramBackButtonText")]
		public string TelegramBackButtonText { get; set; }

		[JsonProperty("TelegramStatusButtonText")]
		public string TelegramStatusButtonText { get; set; }


		[JsonProperty("TelegramStatusNotMemberMessage")]
		public string TelegramStatusNotMemberMessage { get; set; }

		[JsonProperty("TelegramStatusMemberMessage")]
		public string TelegramStatusMemberMessage { get; set; }


		[JsonProperty("TelegramWelcomeText")]
		public string TelegramWelcomeText { get; set; }

		[JsonProperty("TelegramCCNewsMembershipButtonText")]
		public string TelegramCcNewsMembershipButtonText { get; set; }

		[JsonProperty("TelegramCELBMembershipButtonText")]
		public string TelegramCelbMembershipButtonText { get; set; }

		[JsonProperty("TelegramPaymentText")]
		public string TelegramPaymentText { get; set; }

		[JsonProperty("TelegramPaymentSuccessText")]
		public string TelegramPaymentSuccessText { get; set; }
		
		[JsonProperty("TelegramCCNewsPaymentSuccessText")]
		public string TelegramCCNewsPaymentSuccessText { get; set; }
		
		[JsonProperty("TelegramCELBPaymentSuccessText")]
		public string TelegramCELBPaymentSuccessText { get; set; }
	}
}
