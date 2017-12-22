using System;

namespace GLAA.ViewModels.LicenceApplication
{
    public class CountryViewModel : ICheckboxList
    {
        public int Id { get; set; }
        public string Name { get; set; }        
        public bool Checked { get; set; }        
    }
}