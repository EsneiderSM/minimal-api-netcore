using Microsoft.EntityFrameworkCore;

namespace MinimalAPIMovies.Entities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Gender>().Property(g => g.Name).HasMaxLength(50);
        }

        public DbSet<Gender> Gender { get; set; }

    }

}
