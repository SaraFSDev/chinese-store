

using Microsoft.EntityFrameworkCore;
using project.Models;
using project.Models.ModelsDto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace project.Repository
{
    public class ProjectDbContext : IdentityDbContext<IdentityUser>
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options)
            : base(options) { }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<Donator> Donators { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Winning> Winnings { get; set; }
        public DbSet<UserGift> UserGifts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserGift>()
                .HasKey(ug => new { ug.UserId, ug.GiftId });

            modelBuilder.Entity<UserGift>()
                .HasOne(ug => ug.User)
                .WithMany(u => u.UserGifts)
                .HasForeignKey(ug => ug.UserId);

            modelBuilder.Entity<UserGift>()
                .HasOne(ug => ug.Gift)
                .WithMany(g => g.UserGifts)
                .HasForeignKey(ug => ug.GiftId);
        }
    }

}




