using DecorAndHandicraftMerchant.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DecorAndHandicraftMerchant.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SubCategory>()
                .HasOne(sc => sc.Category)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(sc => sc.CategoryId)
                .HasConstraintName("FK_SubCategories_CategoryId");

            builder.Entity<Product>()
                    .HasOne(p => p.SubCategory)
                    .WithMany(sc => sc.Products)
                    .HasForeignKey(p => p.SubCategoryId)
                    .HasConstraintName("FK_Products_SubCategoryId");

            builder.Entity<Cart>()
                   .HasOne(c => c.Product)
                   .WithMany(p => p.Carts)
                   .HasForeignKey(c => c.ProductId)
                   .HasConstraintName("FK_Carts_ProductId");

            builder.Entity<OrderDetail>()
                   .HasOne(od => od.Product)
                   .WithMany(p => p.OrderDetails)
                   .HasForeignKey(od => od.ProductId)
                   .HasConstraintName("FK_OrderDetails_ProductId");
            
            builder.Entity<OrderDetail>()
                   .HasOne(od => od.Order)
                   .WithMany(o => o.OrderDetails)
                   .HasForeignKey(od => od.OrderId)
                   .HasConstraintName("FK_OrderDetails_OrderId");

            builder.Entity<Order>()
                   .HasOne(o => o.Profile)
                   .WithMany(p => p.Orders)
                   .HasForeignKey(o => o.ProfileId)
                   .HasConstraintName("FK_Orders_ProfileId");

            builder.Entity<Order>()
                   .HasOne(o => o.Address)
                   .WithMany(a => a.Orders)
                   .HasForeignKey(o => o.AddressId)
                   .HasConstraintName("FK_Orders_AddressId");

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
    }
}
