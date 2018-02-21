namespace GLAA.ViewModels.LicenceApplication
{
    public interface IPersonViewModel
    {
        bool IsUk { get; set; }

        FullNameViewModel FullName { get; set; }
        AlternativeFullNameViewModel AlternativeName { get; set; }
        DateOfBirthViewModel DateOfBirth { get; set; }
        BirthDetailsViewModel BirthDetailsViewModel { get; set; }
        //TownOfBirthViewModel TownOfBirth { get; set; }
        //CountryOfBirthViewModel CountryOfBirth { get; set; }
        JobTitleViewModel JobTitle { get; set; }
        AddressViewModel Address { get; set; }
        BusinessPhoneNumberViewModel BusinessPhoneNumber { get; set; }
        BusinessExtensionViewModel BusinessExtension { get; set; }
        PersonalEmailAddressViewModel PersonalEmailAddress { get; set; }
        PersonalMobileNumberViewModel PersonalMobileNumber { get; set; }
        //NationalInsuranceNumberViewModel NationalInsuranceNumber { get; set; }

        NationalityViewModel Nationality { get; set; }
        PassportViewModel PassportViewModel { get; set; }
        UndischargedBankruptViewModel UndischargedBankruptViewModel { get; set; }
        DisqualifiedDirectorViewModel DisqualifiedDirectorViewModel { get; set; }
        RestraintOrdersViewModel RestraintOrdersViewModel { get; set; }
        UnspentConvictionsViewModel UnspentConvictionsViewModel { get; set; }
        OffencesAwaitingTrialViewModel OffencesAwaitingTrialViewModel { get; set; }
        PreviousLicenceViewModel PreviousLicenceViewModel { get; set; }
    }
}