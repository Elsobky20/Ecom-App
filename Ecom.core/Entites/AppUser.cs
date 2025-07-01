using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.core.Entites
{
   public class AppUser:IdentityUser
    {
        public string DisaplayName { get; set; }
        public Address Address { get; set; }
    }
}
