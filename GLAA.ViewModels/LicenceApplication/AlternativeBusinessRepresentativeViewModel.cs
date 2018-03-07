namespace GLAA.ViewModels.LicenceApplication
{
    public class AlternativeBusinessRepresentativeViewModel : PersonViewModel
    {
        public AlternativeBusinessRepresentativeViewModel()
        {
            RightToWork = new RightToWorkViewModel();
        }

        public int? Id { get; set; }

        public RightToWorkViewModel RightToWork { get; set; }
    }
}