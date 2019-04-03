using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sw2API.Entities;
using sw2API.Models;

namespace sw2API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<ApplicationRole>().ToTable("Roles");

            
            #region "Seed Data"

            builder.Entity<ApplicationRole>().HasData(
                new
                {
                    Id = "1", Name = "Admin", NormalizedName = "ADMIN",
                    Description = "Admin Account has full control and authorization"
                },
                new
                {
                    Id = "2", Name = "Cashier", NormalizedName = "CASHIER",
                    Description = "Cashier Account has Some Controls Chosen by admin"
                }
            );
            builder.Entity<Customer>().Property(b => b.MembershipTypeId).HasDefaultValue(1);
            builder.Entity<MembershipType>().HasData(
                new
                {
                    MembershipTypeId = 1,
                    Name = "1 Month",
                    SignUpFee = 200,
                    DurationInMonths = (byte)1,
                    SaunaAccess = false,
                    CrossFitAccess = false

                },
                new
                {
                    MembershipTypeId = 2,
                    Name = "3 Month with 10% Discount",
                    SignUpFee = 540,
                    DurationInMonths = (byte)3,
                    SaunaAccess = false,
                    CrossFitAccess = false

                },
                new
                {
                    MembershipTypeId = 3,
                    Name = "6 Month with 20% Discount and CrossFit",
                    SignUpFee = 640,
                    DurationInMonths = (byte)6,
                    SaunaAccess = false,
                    CrossFitAccess = true

                },
                new
                {
                    MembershipTypeId = 4,
                    Name = "1 Year with 30% Discount, CrossFit and Sauna",
                    SignUpFee = 840,
                    DurationInMonths = (byte)12,
                    SaunaAccess = true,
                    CrossFitAccess = true

                }

            );

            #endregion
        }

        public DbSet<sw2API.Models.ApplicationRole> ApplicationRole { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<MembershipType> MembershipTypes { get; set; }

    }
}
