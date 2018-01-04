using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GLAA.Domain.Models
{
    public class GLAAUser : IdentityUser
    {
        public virtual ICollection<Licence> Licences { get; set; }

        //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<GLAAUser> manager)
        //{
        //    // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        //    var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        //    // Add custom user claims here
        //    return userIdentity;
        //}
    }
}
