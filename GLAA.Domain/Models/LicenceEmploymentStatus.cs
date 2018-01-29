using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GLAA.Domain.Models
{
    public class LicenceEmploymentStatus : ICompositeId
    {
        [NotMapped]
        public int Id {
            get { return EmploymentStatusId; }
            set { EmploymentStatusId = value;  }
        }
        public int LicenceId { get; set; }
        public virtual Licence Licence { get; set; }
        public int EmploymentStatusId { get; set; }
        public virtual EmploymentStatus EmploymentStatus { get; set; }
    }
}
