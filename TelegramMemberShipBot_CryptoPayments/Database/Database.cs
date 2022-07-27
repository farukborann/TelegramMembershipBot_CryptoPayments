using TelegramMemberShipBot_CryptoPayments.Context;
using TelegramMemberShipBot_CryptoPayments.Models;

namespace TelegramMemberShipBot_CryptoPayments
{
    public class Database
    {
        private static MainContext MainContext { get; set; }

        public Database()
        {
            MainContext = new();

            if (SettingsManager.ResetDatabaseFirstStart)
            {
                ResetDatabase();
            }
            else if (!SettingsManager.IsDatabaseEnsureCreated)
            {
                CreateDatabase();
            }
        }

        public void CreateDatabase()
        {
            MainContext.Database.EnsureCreated();
            SettingsManager.SetDatabaseEnsureCreated();
            Console.WriteLine("Database Created");
        }
        
        public void ResetDatabase()
        {
            MainContext.Database.EnsureDeleted();
            SettingsManager.DatabaseResetted();
            Console.WriteLine("Database resetted. Please start bot again.");
            Environment.Exit(0);
        }

        //Users
        public List<User> GetExpiredUsers()
        {
            return MainContext.Users.Where(x => x.EndDate < DateTime.Now).ToList();
        }

        public User GetCCNewsStatus(long chatId)
        {
            if (MainContext.Users.Any(x => x.UserId == chatId)) return MainContext.Users.First(x => x.UserId == chatId);
            else return new User() { UserId = -1 };
        }

        public List<User> GetUsers()
        {
            return MainContext.Users.ToList();
        }

        public void AddUser(User user)
        {
            MainContext.Add(user);
            MainContext.SaveChangesAsync();
        }

        public void DelUser(User user)
        {
            MainContext.Users.Remove(user);
        }
        
        //Payment
        public void AddPayment(Payment payment)
        {
            MainContext.Add(payment);
            MainContext.SaveChangesAsync();
        }
        
        public void ComplatePayment(Payment payment)
        {
            payment.ComplateDate = DateTime.Now;
            payment.LastStatus = 1;
            if (payment.ISCCNews && MainContext.Users.Any((User x) => x.UserId == payment.UserId))
            {
                MainContext.Users.First((User x) => x.UserId == payment.UserId).EndDate += TimeSpan.FromDays(SettingsManager.CCNewsMembershipPeriotDays);
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
                AddUser(user);
            }
            MainContext.SaveChangesAsync();
        }

        public void CancelPayment(Payment payment)
        {
            payment.ComplateDate = DateTime.Now;
            payment.LastStatus = -1;
            MainContext.SaveChangesAsync();
        }

        public List<Payment> GetPendingPayments()
        {
            return MainContext.Payments.Where(x => x.LastStatus == 0).ToList();
        }

        public List<Payment> GetLast100Payment()
        {
            if (MainContext.Payments.Count() < 100)
            {
                return MainContext.Payments.ToList().GetRange(0, MainContext.Payments.Count());
            }
            return MainContext.Payments.ToList().GetRange(0, 100);
        }
    }
}