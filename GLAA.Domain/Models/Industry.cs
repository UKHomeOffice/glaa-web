using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLAA.Domain.Core.Models;

namespace GLAA.Domain.Models
{
    public class Industry : ICheckboxListable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<LicenceIndustry> Licences { get; set; }
    }
}
