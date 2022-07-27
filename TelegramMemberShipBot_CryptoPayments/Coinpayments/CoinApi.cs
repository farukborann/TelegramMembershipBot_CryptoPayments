using Flurl.Http;
using System.Text;
using Newtonsoft.Json;
using Flurl.Http.Content;

namespace TelegramMemberShipBot_CryptoPayments.Coinpayments
{
    public class CoinApi
    {
        public readonly string publicApiKey;
        public readonly string privateApiKey;

        public CoinApi(string publicApiKey, string privateApiKey)
        {
            this.publicApiKey = publicApiKey;
            this.privateApiKey = privateApiKey;
        }

        //Base Methods
        private async Task<HttpResponseMessage> SendRequestAsync(BaseTransaction transaction)
        {
            transaction.Key = publicApiKey;
            var query = string.Empty.GetUrl(transaction.ToKeyValuePair());

            Encoding encoding = Encoding.UTF8;
            byte[] keyBytes = encoding.GetBytes(privateApiKey);
            byte[] postBytes = encoding.GetBytes(query);
            var hmacsha512 = new System.Security.Cryptography.HMACSHA512(keyBytes);
            string hmac = BitConverter.ToString(hmacsha512.ComputeHash(postBytes)).Replace("-", string.Empty);
            var _ = await SettingsManager.CoinpaymentsApiUrl.WithHeaders(new Dictionary<string, string> { ["HMAC"] = hmac, ["Content-Type"] = "application/x-www-form-urlencoded" }).PostAsync(new CapturedUrlEncodedContent(query));
            return _.ResponseMessage;
        }

        //Create Transaction Method
        public async Task<ReceiveTransactionResponse> CreateTransactionAsync(ReceiveTransaction transaction)
        {
            var result = await SendRequestAsync(transaction);

            if (result.IsSuccessStatusCode)
            {
                var str = await result.Content.ReadAsStringAsync();
                try
                {
                    var data = JsonConvert.DeserializeObject<ReceiveTransactionResponse>(str);
                    return data;
                }
                catch
                {
                    var data = JsonConvert.DeserializeObject<TransactionException>(str);
                    throw data;
                }
            }
            else throw new TransactionException { Error = result.StatusCode.ToString() };
        }

        //Get Transaction Info Method
        public async Task<ReceiveTransactionInfoResponse> GetTransactionInfoAsync(ReceiveTransactionInfo transaction)
        {
            var result = await SendRequestAsync(transaction);
            if (result.IsSuccessStatusCode)
            {
                var str = await result.Content.ReadAsStringAsync();
                try
                {
                    var data = JsonConvert.DeserializeObject<ReceiveTransactionInfoResponse>(str);
                    return data;
                }
                catch
                {
                    var data = JsonConvert.DeserializeObject<TransactionException>(str);
                    throw data;
                }
            }
            else throw new TransactionException { Error = result.StatusCode.ToString() };
        }

        //Get Transactions Info Method => Max 25 Transaction
        public async Task<ReceiveTransactionsInfoResponse> GetTransactionInfoMultiAsync(ReceiveTransactionsInfo transactions)
        {
            var result = await SendRequestAsync(transactions);
            if (result.IsSuccessStatusCode)
            {
                var str = await result.Content.ReadAsStringAsync();
                try
                {
                    var data = JsonConvert.DeserializeObject<ReceiveTransactionsInfoResponse>(str);
                    return data;
                }
                catch
                {
                    var data = JsonConvert.DeserializeObject<TransactionException>(str);
                    throw data;
                }
            }
            else throw new TransactionException { Error = result.StatusCode.ToString() };
        }
    }
}
