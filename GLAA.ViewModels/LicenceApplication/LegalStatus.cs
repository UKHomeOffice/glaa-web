using GLAA.Domain.Models;

namespace GLAA.ViewModels.LicenceApplication
{
    public class LegalStatus : ICheckboxList<LegalStatusEnum?>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
        public LegalStatusEnum? EnumMappedTo { get; set; }
    }
}