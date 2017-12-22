namespace GLAA.ViewModels.LicenceApplication
{
    public class LegalStatus : ICheckboxList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
    }
}