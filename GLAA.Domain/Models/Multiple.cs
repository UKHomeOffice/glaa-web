using System.Collections.Generic;
using GLAA.Domain.Core.Models;

namespace GLAA.Domain.Models
{
    public class Multiple : ICheckboxListable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<LicenceMultiple> Licences { get; set; }
    }
}
