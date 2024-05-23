using ActionCommandGame.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace ActionCommandGame.Repository
{
    public class ActionButtonGameDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ActionButtonGameDbContext(DbContextOptions<ActionButtonGameDbContext> options): base(options)
        {
            
        }

        public DbSet<PositiveGameEvent> PositiveGameEvents { get; set; }
        public DbSet<NegativeGameEvent> NegativeGameEvents { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerItem> PlayerItems { get; set; }
        public DbSet<IdentityUser> AspNetUsers { get; set; }
        public DbSet<IdentityUserRole<string>> AspNetUserRoles { get; set; }
        public DbSet<IdentityRole> AspNetRoles { get; set; }

    }
}
