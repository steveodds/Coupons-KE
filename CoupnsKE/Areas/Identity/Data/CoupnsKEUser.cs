using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using CoupnsKE.Enum;

namespace CoupnsKE.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the CoupnsKEUser class
    public class CoupnsKEUser : IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }
        [PersonalData]
        public DateTime DOB { get; set; }

        [PersonalData]
        public Gender Gender { get; set; }
        [PersonalData]
        public Roles UserRole { get; set; }
    }
}
