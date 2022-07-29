using Microsoft.EntityFrameworkCore;
using TelegramMemberShipBot_CryptoPayments.Models;

namespace TelegramMemberShipBot_CryptoPayments.Database
{
    public class DatabaseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(SettingsManager.DatabaseConnectionString);
        }

        public void CheckDatabaseSettings()
        {
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
            Database.EnsureCreated();
            SettingsManager.SetDatabaseEnsureCreated();
            Console.WriteLine("| Database Created |");
        }

        public void ResetDatabase()
        {
            Database.EnsureDeleted();
            SettingsManager.DatabaseResetted();
            Console.WriteLine("| Database resetted |\nPlease start bot again. Press a button for exit...");
            Console.ReadKey();
            Environment.Exit(0);
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Payment> Payments { get; set; }
    }
}
