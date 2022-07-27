namespace TelegramMemberShipBot_CryptoPayments.Coinpayments
{
    static class Utils
    {
        public static List<KeyValuePair<string, string>> ToKeyValuePair(this object obj)
        {
            List<KeyValuePair<string, string>> ksv = new List<KeyValuePair<string, string>>();

            foreach (var item in obj.GetType().GetProperties())
            {
                string name = string.Empty;
                var atm = item.GetCustomAttributes(typeof(Newtonsoft.Json.JsonPropertyAttribute), false).FirstOrDefault();
                if (atm != null)
                {
                    name = ((Newtonsoft.Json.JsonPropertyAttribute)atm).PropertyName;
                }
                else name = item.Name;

                if (item.GetValue(obj) != null) ksv.Add(new KeyValuePair<string, string>(name, item.GetValue(obj).ToString()));
            }
            return ksv;
        }
        
        public static List<KeyValuePair<string, string>> ListToKeyValuePair(this List<string> objs, string key)
        {
            List<KeyValuePair<string, string>> ksv = new List<KeyValuePair<string, string>>();

            foreach (var obj in objs)
            {
                if (obj != null) ksv.Add(new KeyValuePair<string, string>(key, obj));
            }
            return ksv;
        }

        public static string GetUrl(this string mainUrl, List<KeyValuePair<string, string>> obj)
        {
            var i = 0;
            foreach (var item in obj)
            {
                if (i == 0) mainUrl += $"{item.Key}={item.Value}";
                else mainUrl += $"&{item.Key}={item.Value}";
                i += 1;
            }
            return mainUrl;
        }
    }
}
