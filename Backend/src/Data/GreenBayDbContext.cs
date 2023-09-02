using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using src.Models;

namespace src.Data
{
    public class GreenBayDbContext : DbContext
    {
        public GreenBayDbContext(DbContextOptions<GreenBayDbContext> options) : base(options)
        { }

        public DbSet<User> TblUsers { get; set; }

        public DbSet<Item> TblItems { get; set; }

        public DbSet<Bid> TblBids { get; set; }

        public DbSet<UserBid> TblUsersBids { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var user1 = new User()
            {
                Id = 1,
                Name = "admin",
                Email = "admin@fox.hu",
                Password = BCrypt.Net.BCrypt.HashPassword("Admin123"),
                Role = "Admin",
                Money = 100.00m,
                CreationDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
            };
            var user2 = new User()
            {
                Id = 2,
                Name = "testuser",
                Email = "testuser@abc.de",
                Password = BCrypt.Net.BCrypt.HashPassword("User123"),
                Role = "User",
                Money = 100.00m,
                CreationDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
            };

            modelBuilder.Entity<UserBid>()
                .HasKey(ub => new { ub.UserId, ub.BidId });

            modelBuilder.Entity<UserBid>()
                .HasOne(ub => ub.User)
                .WithMany(user => user.UserBids)
                .HasForeignKey(ub => ub.UserId);

            modelBuilder.Entity<UserBid>()
                .HasOne(ub => ub.Bid)
                .WithMany()
                .HasForeignKey(ub => ub.BidId);


            modelBuilder.Entity<User>().HasData(user1, user2);

            // var item1 = new Item()
            // {
            //     Id = 1,
            //     Name = "TV Sony",
            //     Description = "An amazing TV",
            //     PhotoUrl = "https://s13emagst.akamaized.net/products/45635/45634164/images/res_fd42def37fbf80666320c5137faccaf1.jpeg",
            //     Price = 30,
            //     Bid = 0,
            //     IsSellable = true,
            //     SellerId = 1,
            //     Seller = user1,
            //     CreationDate = DateTime.UtcNow,
            //     UpdateDate = DateTime.UtcNow.AddDays(1)
            // };
            // var item2 = new Item()
            // {
            //     Id = 2,
            //     Name = "Electrolux Vacum",
            //     Description = "A wanderful vacum.",
            //     PhotoUrl = "https://www.electrolux.com.my/globalassets/appliances/vacuum-clearner/z931-fr-1500x1500.png",
            //     Price = 20,
            //     Bid = 10,
            //     IsSellable = true,
            //     SellerId = 2,
            //     Seller = user2,
            //     CreationDate = DateTime.UtcNow,
            //     UpdateDate = DateTime.UtcNow.AddDays(1)
            // };

            // //Configure the one-to-one relationship between User and Item
            // modelBuilder.Entity<User>()
            //     .HasMany(user => user.ItemsForSale)
            //     .WithOne(item => item.Seller)
            //     .HasForeignKey(item => item.SellerId); // Foreign key property in Item entity

            // modelBuilder.Entity<Item>()
            //     .HasOne(item => item.Seller)
            //     .WithOne()
            //     .HasForeignKey<User>(user => user.Id); // Foreign key property in Item entity

            // modelBuilder.Entity<Item>().HasData(item1, item2);

        }
    }
}