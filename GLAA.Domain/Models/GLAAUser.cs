﻿using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GLAA.Domain.Models
{
    public class GLAAUser : IdentityUser, IAddressable
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string FullName => (string.IsNullOrEmpty(Title) ? string.Empty : $"{Title} ") +
                                  (string.IsNullOrEmpty(FirstName) ? string.Empty : $"{FirstName} ") +
                                  (string.IsNullOrEmpty(MiddleName) ? string.Empty : $"{MiddleName} ") +
                                  (string.IsNullOrEmpty(LastName) ? string.Empty : $"{LastName} ");

        public int? AddressId { get; set; }
        [ForeignKey(nameof(AddressId))]
        public virtual Address Address { get; set; }

        public CommunicationPreference? CommunicationPreference { get; set; }

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
