using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SignalRChatApp.Models;

namespace SignalRChatApp.Repository
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<RoomUser> RoomsUsers { get; set; }

        public DbSet<BannedUser> BannedUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RoomUser>().HasKey(t => new { t.RoomId, t.UserId });

            builder.Entity<BannedUser>().HasKey(t => new { t.RoomId, t.UserId });
        }
    }
}
