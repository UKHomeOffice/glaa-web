namespace GLAA.ViewModels.LicenceApplication
{
    public class AlternativeBusinessRepresentativeViewModel : PersonViewModel
    {
        public AlternativeBusinessRepresentativeViewModel()
        {
            RightToWorkViewModel = new RightToWorkViewModel();
        }

        public int? Id { get; set; }

        public RightToWorkViewModel RightToWorkViewModel { get; set; }
    }
}