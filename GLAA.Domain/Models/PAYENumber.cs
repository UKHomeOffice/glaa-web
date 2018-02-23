using System;

namespace GLAA.Domain.Models
{
    public class PAYENumber : IId, ILinkedToLicence, IDeletable
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public DateTime RegistrationDate { get; set; }

        public virtual Licence Licence { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}
