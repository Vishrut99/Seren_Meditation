using Microsoft.EntityFrameworkCore;

namespace MEDIT.Models
{
    public class MeditDbContext : DbContext
    {
        public MeditDbContext(DbContextOptions<MeditDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<ProgressTrackings> ProgressTrackings { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=medit.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.User)
                .WithMany(u => u.Subscriptions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}