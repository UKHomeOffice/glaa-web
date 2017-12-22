using System.ComponentModel.DataAnnotations;

namespace GLAA.Domain.Models
{
    public class Address : IId
    {                
        [Key]
        public int Id { get; set; }

        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string Town { get; set; }

        public string County { get; set; }

        public string Postcode { get; set; }

        public string Country { get; set; }

        public bool NonUK { get; set; }            
    }
}
