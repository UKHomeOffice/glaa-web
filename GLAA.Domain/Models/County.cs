using System.Collections.Generic;

namespace GLAA.Domain.Models
{
    public class County : ICheckboxListable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
    }
}
