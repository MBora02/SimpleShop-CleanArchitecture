using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleShop.Domain.Entities
{
    public class AppUser: IdentityUser
    {
        public string FullName { get; set; }=string.Empty;
    }
}
