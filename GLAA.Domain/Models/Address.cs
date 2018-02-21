using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public string Postcode { get; set; }

        public int? CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        public virtual Country Country { get; set; }

        public int? CountyId { get; set; }
        [ForeignKey(nameof(CountyId))]
        public virtual County County { get; set; }        
    }
}
