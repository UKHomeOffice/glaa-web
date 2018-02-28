using System;

namespace GLAA.Domain.Models
{
    public class OffenceAwaitingTrial : IId, IDeletable
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime? Date { get; set; }
        
        public virtual PrincipalAuthority PrincipalAuthority { get; set; }
        
        public virtual AlternativeBusinessRepresentative AlternativeBusinessRepresentative { get; set; }
        
        public virtual DirectorOrPartner DirectorOrPartner { get; set; }

        public virtual NamedIndividual NamedIndividual { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}
