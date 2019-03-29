using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace sw2API.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public  ApplicationUser() : base() { }
        public override string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
