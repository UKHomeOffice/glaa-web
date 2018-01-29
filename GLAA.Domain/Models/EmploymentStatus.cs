using System.Collections.Generic;
using GLAA.Domain.Core.Models;

namespace GLAA.Domain.Models
{
    public class EmploymentStatus : ICheckboxListable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<LicenceEmploymentStatus> Licences { get; set; }
    }
}
