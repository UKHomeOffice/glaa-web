namespace GLAA.ViewModels.LicenceApplication
{
    public interface IPersonViewModel
    {
        bool IsUk { get; set; }

        FullNameViewModel FullName { get; set; }
        AlternativeFullNameViewModel AlternativeName { get; set; }
        DateOfBirthViewModel DateOfBirth { get; set; }
        BirthDetailsViewModel BirthDetails { get; set; }
        JobTitleViewModel JobTitle { get; set; }
        AddressViewModel Address { get; set; }
        BusinessPhoneNumberViewModel BusinessPhoneNumber { get; set; }
        BusinessExtensionViewModel BusinessExtension { get; set; }
        PersonalEmailAddressViewModel PersonalEmailAddress { get; set; }
        PersonalMobileNumberViewModel PersonalMobileNumber { get; set; }

        NationalityViewModel Nationality { get; set; }
        PassportViewModel Passport { get; set; }
        UndischargedBankruptViewModel UndischargedBankrupt { get; set; }
        DisqualifiedDirectorViewModel DisqualifiedDirector { get; set; }
        RestraintOrdersViewModel RestraintOrders { get; set; }
        UnspentConvictionsViewModel UnspentConvictions { get; set; }
        OffencesAwaitingTrialViewModel OffencesAwaitingTrial { get; set; }
        PreviousLicenceViewModel PreviousLicence { get; set; }
    }
}