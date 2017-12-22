using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using GLAA.Domain.Models;

namespace GLAA.Domain.Core.Models
{
    public class LicenceSector : ICompositeId
    {
        [NotMapped]
        public int Id
        {
            get { return SectorId; }
            set { SectorId = value; }
        }
        public int LicenceId { get; set; }
        public virtual Licence Licence { get; set; }
        public int SectorId { get; set; }
        public virtual Sector Sector { get; set; }
    }
}
