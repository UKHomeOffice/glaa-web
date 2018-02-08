using GLAA.Domain.Models;
using GLAA.ViewModels.Core;
using GLAA.ViewModels.Core.Attributes;
using System.ComponentModel.DataAnnotations;

namespace GLAA.ViewModels.LicenceApplication
{
    public class CompaniesHouseRegistrationViewModel : Validatable, IRequiredIf
    {
        public CompaniesHouseRegistrationViewModel()
        {
            CompanyRegistrationDate = new DateViewModel();
        }

        // TODO: Check example numbers
        [RegularExpression(@"\w{2}\d{6}", ErrorMessage = "Companies House registration number")]
        [RequiredIf(ErrorMessage = "The Companies House Registration Number is required")]
        [Display(Name = "Companies House Registration Number", Description = "For example 01234567 or OC012345")]
        public string CompaniesHouseNumber { get; set; }

        [RequiredIf(ErrorMessage = "The Registration Date field is required")]
        [UIHint("_NullableDateTime")]
        [Display(Name = "Registration Date", Description = "")]
        public DateViewModel CompanyRegistrationDate { get; set; }

        public LegalStatusEnum LegalStatus { get; set; }

        public bool IsRequired => LegalStatus == LegalStatusEnum.RegisteredCompany;
    }
}
