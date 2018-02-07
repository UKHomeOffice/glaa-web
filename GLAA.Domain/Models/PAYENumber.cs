using System;

namespace GLAA.Domain.Models
{
    public class PAYENumber : IId
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public DateTime RegistrationDate { get; set; }

        public virtual Licence Licence { get; set; }
    }
}
