using System;

namespace GLAA.ViewModels.File
{
    public class FileUploadedViewModel
    {
        public string Key { get; set; }
        //TODO - a more appropriate way to store any errors?
        public Exception Exception { get; set; }
        public string ETag{ get; set; }
        public string VersionId { get; set; }
        public string FileName { get; set; }
    }
}
