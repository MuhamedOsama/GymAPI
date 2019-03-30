using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                new { Id = "1", Name = "Admin", NormalizedName = "ADMIN", Description = "Admin Account has full control and authorization" },
                new { Id = "2", Name = "Cashier", NormalizedName = "CASHIER", Description = "Cashier Account has Some Controls Chosen by admin" }
            );
            
            #endregion
        }

        public DbSet<sw2API.Models.ApplicationRole> ApplicationRole { get; set; }
    }
}
