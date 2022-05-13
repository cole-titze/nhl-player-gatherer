using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class PlayerDbContext : DbContext
    {
        public DbSet<PlayerValue> PlayerValues { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\;Database=Players;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
