using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Src.Models;

namespace Src.Data
{
    public class GreenBayDbContext : DbContext
    {
        public GreenBayDbContext(DbContextOptions<GreenBayDbContext> options) : base(options)
        { }

        public DbSet<User> TblUsers { get; set; }

        public DbSet<Item> TblItems { get; set; }

        public DbSet<Bid> TblBids { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = 1,
                    Name = "admin",
                    Email = "admin@fox.hu",
                    Password = BCrypt.Net.BCrypt.HashPassword("Admin123"),
                    Role = "Admin",
                    Money = 100.00m,
                    CreationDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                },
                new User()
                {
                    Id = 2,
                    Name = "testuser",
                    Email = "testuser@abc.de",
                    Password = BCrypt.Net.BCrypt.HashPassword("User123"),
                    Role = "User",
                    Money = 100.00m,
                    CreationDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                }

            );
            modelBuilder.Entity<Item>().HasData(
                new Item()
                {
                    Id = 1,
                    Name = "TV Sony",
                    Description = "An amazing TV",
                    PhotoUrl = "https://s13emagst.akamaized.net/products/45635/45634164/images/res_fd42def37fbf80666320c5137faccaf1.jpeg",
                    Price = 30,
                    Bid = 0,
                    IsSellable = true,
                    SellerId = 1,
                    CreationDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow.AddDays(1)

                },
                new Item()
                {
                    Id = 2,
                    Name = "Electrolux Vacum",
                    Description = "A wanderful vacum.",
                    PhotoUrl = "https://www.electrolux.com.my/globalassets/appliances/vacuum-clearner/z931-fr-1500x1500.png",
                    Price = 20,
                    Bid = 0,
                    IsSellable = true,
                    SellerId = 2,
                    CreationDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow.AddDays(1)
                }
            );

            modelBuilder.Entity<User>(user =>
            {
                user.HasKey(u => u.Id);
            });

            // Using Core Fluent API to configure dabase schema and specify relation ships between entities.
            // Defining the mapping between the object-oriented model and the underlying database.
            modelBuilder.Entity<Item>(item =>
            {
                item.HasKey(i => i.Id);
                item.HasOne<User>(i => i.Seller)
                    .WithMany(u => u.SellingItems)
                    .HasForeignKey(i => i.SellerId)
                    .IsRequired();
                item.HasOne<User>(i => i.Buyer)
                    .WithMany(u => u.BoughtItems)
                    .HasForeignKey(i => i.BuyerId);
            });

            modelBuilder.Entity<Bid>(bid =>
            {
                bid.HasKey(b => b.Id);
                bid.Property(b => b.BidAmount).IsRequired().HasColumnType("decimal(10,2)");
                bid.HasOne<User>(b => b.Bider)
                    .WithMany(u => u.Bids)
                    .HasForeignKey(b => b.BiderId)
                    .IsRequired();
                bid.HasOne<Item>(b => b.Item)
                    .WithMany(i => i.Bids)
                    .HasForeignKey(b => b.ItemId)
                    .IsRequired();
            });
        }
    }
}