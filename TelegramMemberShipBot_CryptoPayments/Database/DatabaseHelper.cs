using TelegramMemberShipBot_CryptoPayments.Models;

namespace TelegramMemberShipBot_CryptoPayments.Database
{
    public static class DatabaseHelper
    {
        //Users
        public static void DelUser(this DatabaseContext DbContext, User user)
        {
            DbContext.Remove(user);
        }

        public static List<User> GetExpiredUsers(this DatabaseContext Context)
        {
            return Context.Users.Where(x => x.EndDate <= DateTime.Now).ToList();
        }

        public static User GetCCNewsStatus(this DatabaseContext Context, long chatId)
        {
            if (Context.Users.Any(x => x.UserId == chatId)) return Context.Users.First(x => x.UserId == chatId);
            else return new User() { UserId = -1 }; // -1 mean is not member
        }

        public static List<User> GetUsers(this DatabaseContext Context)
        {
            return Context.Users.ToList();
        }

        //Payment
        public static void AddPayment(this DatabaseContext Context, Payment payment)
        {
            Context.Add(payment);
        }

        public static void ComplatePayment(this DatabaseContext Context, Payment payment, string StatusText)
        {
            payment.ComplateDate = DateTime.Now;
            payment.LastStatus = StatusText;
            if (payment.ISCCNews && Context.Users.Any((x) => x.UserId == payment.UserId))
            {
                Context.Users.First((x) => x.UserId == payment.UserId).EndDate += TimeSpan.FromDays(SettingsManager.CCNewsMembershipPeriotDays);
            }
            else if (payment.ISCCNews)
            {
                User user = new()
                {
                    UserId = payment.UserId,
                    Username = payment.Username,
                    FullName = payment.FullName,
                    LastPaymentCoinName = payment.Coin,
                    LastPaymentAmountCoin = payment.AmountCoin,
                    LastPaymentAmountMain = payment.AmountMain,
                    EndDate = DateTime.Now + TimeSpan.FromDays(SettingsManager.CCNewsMembershipPeriotDays)
                };
                Context.Add(user);
            }
        }

        public static void CancelPayment(this DatabaseContext Context, Payment payment)
        {
            payment.ComplateDate = DateTime.Now;
            payment.LastStatus = "Cancelled / Timed Out";

            Context.Update(payment);
        }

        public static List<Payment> GetPendingPayments(this DatabaseContext Context)
        {
            return Context.Payments.Where(x => x.LastStatus == "Waiting for buyer funds...").ToList();
        }

        public static List<Payment> GetLast100Payment(this DatabaseContext Context)
        {
            if (Context.Payments.Count() < 100)
            {
                return Context.Payments.ToList().GetRange(0, Context.Payments.Count());
            }
            return Context.Payments.ToList().GetRange(0, 100);
        }
    }
}