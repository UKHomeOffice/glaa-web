using GLAA.Domain.Models;

namespace GLAA.ViewModels.LicenceApplication
{
    public class PermissionToWork : ICheckboxList<PermissionToWorkEnum?>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
        public PermissionToWorkEnum? EnumMappedTo { get; set; }
    }
}
