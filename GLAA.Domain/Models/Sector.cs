using System.Collections.Generic;
using GLAA.Domain.Core.Models;

namespace GLAA.Domain.Models
{
    public class Sector : ICheckboxListable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<LicenceSector> Licences { get; set; }
    }
}
