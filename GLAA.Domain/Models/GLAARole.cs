using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace GLAA.Domain.Models
{
    public class GLAARole : IdentityRole
    {
        public GLAARole() : base() { }
        public GLAARole(string roleName) : base(roleName)
        {
            Name = roleName;
            Description = roleName;
        }

        public GLAARole(string roleName, string description = "") : base(roleName)
        {
            Name = roleName;
            Description = description;
        }

        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
