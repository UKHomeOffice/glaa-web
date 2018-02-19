using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GLAA.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.ViewModels.LicenceApplication
{
    public class AddressViewModel : IValidatable, IId, INeedCountries, INeedCounties
    {
        public AddressViewModel()
        {
            Countries = new List<SelectListItem>();
            Counties = new List<SelectListItem>();
        }

        public int Id { get; set; }
        [Required]
        [Display(Name = "Postcode")]
        public string Postcode { get; set; }
        [Required]
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }
        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }
        [Display(Name = "Address Line 3")]
        public string AddressLine3 { get; set; }
        [Required]
        public string Town { get; set; }
        [Required]
        [Display(Name = "County")]
        public int CountyId { get; set; }
        [Required]
        [Display(Name = "Country")]
        public int CountryId { get; set; }
        [Display(Name = "Non UK Address")]
        public bool NonUK { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }
        public IEnumerable<SelectListItem> Counties { get; set; }

        public void Validate()
        {
            if (NonUK)
            {
                IsValid = !string.IsNullOrEmpty(AddressLine1) &&
                          !string.IsNullOrEmpty(AddressLine2) &&
                          !string.IsNullOrEmpty(Postcode);
            }
            else
            {
                IsValid = !string.IsNullOrEmpty(AddressLine1) &&
                          !string.IsNullOrEmpty(AddressLine2) &&
                          !string.IsNullOrEmpty(Postcode) &&
                          !string.IsNullOrEmpty(Town);
            }
        }

        public bool IsValid { get; set; }
    }

    public class AddressPageViewModel : INeedCountries, INeedCounties, IValidatable
    {
        public AddressPageViewModel()
        {
            Address = new AddressViewModel();
        }

        [Required]
        [Display(Name = "Postcode")]
        public string PostCodeLookup { get; set; }

        public AddressViewModel Address { get; set; }

        public IEnumerable<SelectListItem> Countries
        {
            get => Address.Countries;
            set => Address.Countries = value;
        }

        public IEnumerable<SelectListItem> Counties
        {
            get => Address.Counties;
            set => Address.Counties = value;
        }
        public void Validate()
        {
            Address?.Validate();

            if (string.IsNullOrEmpty(PostCodeLookup) && (!Address?.IsValid ?? false))
            {
                IsValid = false;
            }

            IsValid = true;
        }

        public bool IsValid { get; set; }
    }
}