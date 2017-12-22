using System;
using System.ComponentModel.DataAnnotations;

namespace GLAA.Domain.Models
{
    public class RestraintOrder : IId
    {
        [Key]
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime? Date { get; set; }

        public virtual PrincipalAuthority PrincipalAuthority { get; set; }

        public virtual AlternativeBusinessRepresentative AlternativeBusinessRepresentative { get; set; }

        public virtual DirectorOrPartner DirectorOrPartner { get; set; }

        public virtual NamedIndividual NamedIndividual { get; set; }
    }
}
