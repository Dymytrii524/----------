using Microsoft.EntityFrameworkCore;

namespace GuestBook.Models
{
    public class GuestBookContext : DbContext
    {
        public GuestBookContext(DbContextOptions<GuestBookContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Message>()
              .Property(m => m.MessageText)
              .HasColumnName("Message");
        }
    }
}
