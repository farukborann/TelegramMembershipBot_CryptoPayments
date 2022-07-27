using Newtonsoft.Json;

namespace TelegramMemberShipBot_CryptoPayments.Coinpayments
{
    //Base Models
    public abstract class BaseTransaction
    {
        [JsonProperty("version")]
        public int Version { get; set; } = 1;

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("cmd")]
        public virtual string Cmd { get; set; }
    }

    public class TransactionException : Exception
    {
        public override string Message => Error;

        [JsonProperty("error")]
        public string Error { get; set; }
    }

    //Create Transaction Models
    public class ReceiveTransaction : BaseTransaction
    {
        [JsonProperty("cmd")]
        public override string Cmd { get; set; } = "create_transaction";

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("currency1")]
        public string Currency1 { get; set; } = "BTC";

        [JsonProperty("currency2")]
        public string Currency2 { get; set; } = "BTC";

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("buyer_email")]
        public string BuyerEmail { get; set; }

        [JsonProperty("buyer_name")]
        public string BuyerName { get; set; }

        [JsonProperty("item_name")]
        public string ItemName { get; set; }

        [JsonProperty("item_number")]
        public string ItemNumber { get; set; }

        [JsonProperty("invoice")]
        public string Invoice { get; set; }

        [JsonProperty("custom")]
        public string Custom { get; set; }

        [JsonProperty("ipn_url")]
        public string CallBackUrl { get; set; }
    }

    public class ReceiveTransactionResponse
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("result")]
        public ReceiveTransactionResult Result { get; set; }
    }

    public class ReceiveTransactionResult
    {
        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("txn_id")]
        public string TransactionId { get; set; }

        [JsonProperty("confirms_needed")]
        public int ConfirmsNeeded { get; set; }

        [JsonProperty("timeout")]
        public int Timeout { get; set; }

        [JsonProperty("status_url")]
        public string StatusUrl { get; set; }

        [JsonProperty("qrcode_url")]
        public string QrCodeUrl { get; set; }
    }

    //Get Transaction Info Models
    public class ReceiveTransactionInfo : BaseTransaction
    {
        [JsonProperty("cmd")]
        public override string Cmd { get; set; } = "get_tx_info";

        [JsonProperty("txid")]
        public string TransactionId { get; set; }
    }

    public class ReceiveTransactionInfoResponse
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("result")]
        public ReceiveTransactionInfoResult Result { get; set; }
    }

    public class ReceiveTransactionInfoResult
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("time_created")]
        public long TimeCreated { get; set; }

        [JsonProperty("time_expires")]
        public long TimeExpires { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("status_text")]
        public string StatusText { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("coin")]
        public string Coin { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("amountf")]
        public string Amountf { get; set; }

        [JsonProperty("received")]
        public long Received { get; set; }

        [JsonProperty("receivedf")]
        public string Receivedf { get; set; }

        [JsonProperty("recv_confirms")]
        public long RecvConfirms { get; set; }

        [JsonProperty("payment_address")]
        public string PaymentAddress { get; set; }

        [JsonProperty("time_completed", NullValueHandling = NullValueHandling.Ignore)]
        public long? TimeCompleted { get; set; }
    }

    //Get Transactions Info Models
    public class ReceiveTransactionsInfo : BaseTransaction
    {
        [JsonProperty("cmd")]
        public override string Cmd { get; set; } = "get_tx_info_multi";

        [JsonProperty("txid")]
        public string TransactionIds { get; set; }
    }

    public class ReceiveTransactionsInfoResponse
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("result")]
        public Dictionary<string, ReceiveTransactionInfoResult> Result { get; set; }
    }

    //Withdrawal
    public class SendTransaction : BaseTransaction
    {
        [JsonProperty("cmd")]
        public override string Cmd { get; set; } = "create_withdrawal";

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; } = "BTC";

        [JsonProperty("currency2")]
        public string Currency2 { get; set; } = "BTC";

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("dest_tag")]
        public string DestTag { get; set; }

        [JsonProperty("ipn_url")]
        public string CallbackUrl { get; set; }

        [JsonProperty("auto_confirm")]
        public int AutoConfirm { get; set; } = 1;

        [JsonProperty("note")]
        public string Note { get; set; }
    }

    public class SendTransactionResponse
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("result")]
        public SendTransactionResult Result { get; set; }
    }

    public class SendTransactionResult
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }
    }
}
