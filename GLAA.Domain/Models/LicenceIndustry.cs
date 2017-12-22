using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using GLAA.Domain.Models;

namespace GLAA.Domain.Core.Models
{
    public class LicenceIndustry : ICompositeId
    {
        [NotMapped]
        public int Id
        {
            get { return IndustryId; }
            set { IndustryId = value; }
        }        
        public int LicenceId { get; set; }
        public virtual Licence Licence { get; set; }
        public int IndustryId { get; set; }
        public virtual Industry Industry { get; set; }
    }
}
