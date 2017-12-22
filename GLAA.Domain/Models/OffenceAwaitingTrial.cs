using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLAA.Domain.Models
{
    public class OffenceAwaitingTrial : IId
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime? Date { get; set; }
        
        public virtual PrincipalAuthority PrincipalAuthority { get; set; }
        
        public virtual AlternativeBusinessRepresentative AlternativeBusinessRepresentative { get; set; }
        
        public virtual DirectorOrPartner DirectorOrPartner { get; set; }

        public virtual NamedIndividual NamedIndividual { get; set; }
    }
}
