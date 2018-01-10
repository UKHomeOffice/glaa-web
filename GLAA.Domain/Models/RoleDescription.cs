using System.ComponentModel.DataAnnotations;

namespace GLAA.Domain.Models
{
    public class RoleDescription : IId
    {
        public string Name { get; set; }
        public string ReadableName { get; set; }
        public string Description { get; set; }
        [Key]
        public int Id { get; set; }
    }
}
