using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace GLAA.Domain.Models
{
    public class GLAAUser : IdentityUser
    {
        public string FullName { get; set; }

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
