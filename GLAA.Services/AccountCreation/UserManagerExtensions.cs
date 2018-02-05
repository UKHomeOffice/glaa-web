using System;
using System.Linq;
using GLAA.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GLAA.Services.AccountCreation
{
    public static class UserManagerExtensions
    {
        public static GLAAUser FindCompleteUserByEmail(this UserManager<GLAAUser> um, string email)
        {
            return um.Users.Include(u => u.Address).SingleOrDefault(x =>
                x.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
