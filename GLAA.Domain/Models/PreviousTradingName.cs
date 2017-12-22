using System.ComponentModel.DataAnnotations;

namespace GLAA.Domain.Models
{
    public class PreviousTradingName : IId
    {
        [Key]
        public int Id { get; set; }

        public string BusinessName { get; set; }

        public string Town { get; set; }

        public string Country { get; set; }

        public virtual PrincipalAuthority PrincipalAuthority { get; set; }
    }
}
