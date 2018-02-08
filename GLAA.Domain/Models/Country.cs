using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GLAA.Domain.Models
{
    public class Country : ICheckboxListable
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}
