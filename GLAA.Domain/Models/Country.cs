using System.Collections.Generic;
using GLAA.Domain.Core.Models;

namespace GLAA.Domain.Models
{
    public class Country : ICheckboxListable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<LicenceCountry> Licences { get; set; }
    }
}
