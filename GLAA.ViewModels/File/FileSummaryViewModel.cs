using GLAA.ViewModels.LicenceApplication;

namespace GLAA.ViewModels.File
{
    public class FileSummaryViewModel : ValidatableYesNoViewModel
    {
        public string Key { get; set; }
        public byte[] Preview { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public bool CorrectFile { get; set; }
    }
}
