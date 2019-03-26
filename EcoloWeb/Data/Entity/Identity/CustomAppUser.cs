using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace EcoloWeb.Data.Entity.Identity
{
    public class ApplicationUser : IdentityUser
    {


        [PersonalData]
        public string Name { get; set; }

        [PersonalData]
        public string Lastname { get; set; }

        [PersonalData]
        public DateTime DOB { get; set; }

        [PersonalData]
        public string PhoneNumer { get; set; }

        //public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
        //public virtual ICollection<ApplicationUserLogin> Logins { get; set; }
        //public virtual ICollection<ApplicationUserToken> Tokens { get; set; }

        [NotMapped]
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();

    }

    public static class GenericPrincipalExtensions
    {
        public static string FullName(this IPrincipal user)
        {
            if (!user.Identity.IsAuthenticated) return "";

            ClaimsIdentity claimsIdentity = user.Identity as ClaimsIdentity;

            var _fullname = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "FullName");

            if (_fullname == null) return "";

            return _fullname.Value;

            
        }
    }

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
        {
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
        }

        [NotMapped]
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();

        [NotMapped]
        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; } = new List<ApplicationRoleClaim>();


    }

    public class ApplicationUserRole : IdentityUserRole<string>
    {
        [NotMapped]
        public virtual ApplicationUser User { get; set; }
        [NotMapped]
        public virtual ApplicationRole Role { get; set; }
    }

    public class ApplicationUserClaim : IdentityUserClaim<string>
    {
        // public virtual AppCustomIdentityUser User { get; set; }
    }

    public class ApplicationUserLogin : IdentityUserLogin<string>
    {
        //        public virtual AppCustomIdentityUser User { get; set; }
    }

    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        //public virtual ApplicationRole Role { get; set; }
    }

    public class ApplicationUserToken : IdentityUserToken<string>
    {
        //      public virtual AppCustomIdentityUser User { get; set; }
    }
}
