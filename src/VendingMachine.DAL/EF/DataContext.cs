using Microsoft.EntityFrameworkCore;
using VendingMachine.DAL.Entities;

namespace VendingMachine.DAL.EF
{
    /// <summary>
    /// Represents context and all tables from database 
    /// </summary>
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> opts) : base(opts) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Drink> Drinks { get; set; }
        public DbSet<Coin> Coins { get; set; }
    }
}
