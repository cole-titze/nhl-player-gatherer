using Microsoft.EntityFrameworkCore;
namespace DataAccess.Models
{
    public partial class PlayerDbContext : DbContext
    {
        private readonly string _connectionString;
        public PlayerDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        public virtual DbSet<PlayerValue> PlayerValue { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
