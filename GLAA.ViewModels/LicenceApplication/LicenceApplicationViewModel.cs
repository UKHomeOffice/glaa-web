using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GLAA.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.ViewModels.LicenceApplication
{
    public class LicenceApplicationViewModel : YesNoViewModel, IViewModel<Licence>, IValidatable, INeedCountries, INeedCounties
    {
        public LicenceApplicationViewModel()
        {
            Declaration = new DeclarationViewModel();
            OrganisationDetails = new OrganisationDetailsViewModel();
            PrincipalAuthority = new PrincipalAuthorityViewModel();
            AlternativeBusinessRepresentatives = new AlternativeBusinessRepresentativeCollectionViewModel();
            DirectorOrPartner = new DirectorOrPartnerCollectionViewModel();
            NamedIndividuals = new NamedIndividualCollectionViewModel();
            Organisation = new OrganisationViewModel();
            NewLicenceStatus = null;
        }

        // Use these to track progress through form for validation purposes
        public string LastSection { get; set; }
        public int LastPage { get; set; }

        public int Id { get; set; }

        public bool IsApplication { get; set; }
        public bool IsLicence => !IsApplication;
        public string ApplicationId { get; set; }

        // TODO: What if they click no?
        [Required(ErrorMessage = "You must agree to the terms and conditions in order to submit your application.")]
        [Display(Name = "I agree")]
        public bool AgreedToTermsAndConditions { get; set; }

        public bool HasAlternativeBusinessRepresentatives { get; set; }

        public NamedIndividualType NamedIndividualType { get; set; }
        
        public DeclarationViewModel Declaration { get; set; }
        public OrganisationDetailsViewModel OrganisationDetails { get; set; }
        public PrincipalAuthorityViewModel PrincipalAuthority { get; set; }
        public AlternativeBusinessRepresentativeCollectionViewModel AlternativeBusinessRepresentatives { get; set; }
        public DirectorOrPartnerCollectionViewModel DirectorOrPartner { get; set; }        
        public NamedIndividualCollectionViewModel NamedIndividuals { get; set; }
        public OrganisationViewModel Organisation { get; set; }

        public int ApplicationFee { get; set; }
        public int InspectionFee { get; set; }

        public LicenceStatusViewModel NewLicenceStatus { get; set; }
        public void Validate()
        {
            OrganisationDetails?.Validate();
            PrincipalAuthority?.Validate();
            AlternativeBusinessRepresentatives?.Validate();
            DirectorOrPartner?.Validate();
            NamedIndividuals?.Validate();
            Organisation?.Validate();

            IsValid = OrganisationDetails != null && OrganisationDetails.IsValid &&
                      PrincipalAuthority != null && PrincipalAuthority.IsValid &&
                      AlternativeBusinessRepresentatives != null && AlternativeBusinessRepresentatives.IsValid &&
                      DirectorOrPartner != null && DirectorOrPartner.IsValid &&
                      NamedIndividuals != null && NamedIndividuals.IsValid &&
                      Organisation != null && Organisation.IsValid;
        }

        public bool IsValid { get; set; }

        public IEnumerable<SelectListItem> Countries
        {
            set => OrganisationDetails.Countries = value;
            get => OrganisationDetails.Countries;
        }

        public IEnumerable<SelectListItem> Counties
        {
            set => OrganisationDetails.Counties = value;
            get => OrganisationDetails.Counties;
        }
    }
}