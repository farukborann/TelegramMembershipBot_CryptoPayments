using Microsoft.EntityFrameworkCore;
using TelegramMemberShipBot_CryptoPayments.Models;

namespace TelegramMemberShipBot_CryptoPayments.Context
{
    public class MainContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(SettingsManager.DatabaseConnectionString);
        }
        
        public DbSet<User> Users { get; set; }

        public DbSet<Payment> Payments { get; set; }
    }
}
