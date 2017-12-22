using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using GLAA.Domain.Models;

namespace GLAA.Domain.Core.Models
{
    public class LicenceCountry : ICompositeId
    {
        [NotMapped]
        public int Id
        {
            get { return CountryId; }
            set { CountryId = value; }
        }        
        public int LicenceId { get; set; }
        public virtual Licence Licence { get; set; }        
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
    }
}
