using GLAA.Domain.Models;

namespace GLAA.ViewModels.LicenceApplication
{
    public class AvailableContract : ICheckboxList<WorkerContract?>
    {
        public WorkerContract? EnumMappedTo { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
    }
}
