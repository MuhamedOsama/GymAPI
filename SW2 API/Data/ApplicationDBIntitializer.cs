using Microsoft.AspNetCore.Identity;
using sw2API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sw2API.Entities;

namespace sw2API.Data
{
    public static class ApplicationDbInitializer
    {

        public static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            
            if (userManager.FindByNameAsync("admin").Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@gym.com"
                };
                IdentityResult result = userManager.CreateAsync(user, "55555555").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }

    }
}
