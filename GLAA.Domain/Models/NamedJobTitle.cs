using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GLAA.Domain.Models
{
    public class NamedJobTitle : IId, ILinkedToLicence
    {
        [Key]
        public int Id { get; set; }

        public string JobTitle { get; set; }

        public int? JobTitleNumber { get; set; }

        public int LicenceId { get; set; }

        [ForeignKey(nameof(LicenceId))]
        public virtual Licence Licence { get; set; }
    }
}
