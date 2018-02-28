namespace GLAA.ViewModels.LicenceApplication
{
    public class NamedIndividualViewModel : Validatable, IIsSubmitted
    {
        public NamedIndividualViewModel()
        {
            FullName = new FullNameViewModel();
            DateOfBirth = new DateOfBirthViewModel();
            BusinessPhoneNumber = new BusinessPhoneNumberViewModel();
            BusinessExtension = new BusinessExtensionViewModel();
            RightToWorkViewModel = new RightToWorkViewModel();
            UndischargedBankruptViewModel = new UndischargedBankruptViewModel();
            DisqualifiedDirectorViewModel = new DisqualifiedDirectorViewModel();
            RestraintOrdersViewModel = new RestraintOrdersViewModel();
            UnspentConvictionsViewModel = new UnspentConvictionsViewModel();
            OffencesAwaitingTrialViewModel = new OffencesAwaitingTrialViewModel();
            PreviousLicenceViewModel = new PreviousLicenceViewModel();
        }

        public int? Id { get; set; }
        public FullNameViewModel FullName { get; set; }
        public DateOfBirthViewModel DateOfBirth{ get; set; }
        public BusinessPhoneNumberViewModel BusinessPhoneNumber { get; set; }
        public BusinessExtensionViewModel BusinessExtension { get; set; }

        public RightToWorkViewModel RightToWorkViewModel { get; set; }
        public UndischargedBankruptViewModel UndischargedBankruptViewModel { get; set; }
        public DisqualifiedDirectorViewModel DisqualifiedDirectorViewModel { get; set; }
        public RestraintOrdersViewModel RestraintOrdersViewModel { get; set; }
        public UnspentConvictionsViewModel UnspentConvictionsViewModel { get; set; }
        public OffencesAwaitingTrialViewModel OffencesAwaitingTrialViewModel { get; set; }
        public PreviousLicenceViewModel PreviousLicenceViewModel { get; set; }
        public bool IsSubmitted { get; set; }
    }
}