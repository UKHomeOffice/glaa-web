using Microsoft.AspNetCore.Mvc;

namespace GLAA.ViewModels.File
{
    public class FileSummaryViewModel
    {
        public string Key { get; set; }
        public byte[] Preview { get; set; }
        public string FileName { get; set; }
    }
}
