using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GLAA.Domain.Models;
using GLAA.ViewModels.Core;
using GLAA.ViewModels.Core.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.ViewModels.LicenceApplication
{
    public class AddressViewModel : IId, INeedCountries, INeedCounties, IRequiredIf
    {
        public AddressViewModel()
        {
            Countries = new List<SelectListItem>();
            Counties = new List<SelectListItem>();
        }

        public int Id { get; set; }

        [RequiredIf(ErrorMessage = "The Postcode field is required.")]
        [Display(Name = "Postcode")]
        public string Postcode { get; set; }

        [Required]
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }
        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }
        [Display(Name = "Address Line 3")]
        public string AddressLine3 { get; set; }

        [RequiredIf(ErrorMessage = "The Town field is required.")]
        public string Town { get; set; }

        [RequiredIf(ErrorMessage = "The County field is required.")]
        [Display(Name = "County")]
        public int CountyId { get; set; }

        [Required]
        [Display(Name = "Country")]
        public int CountryId { get; set; }
        
        public bool NonUK { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }
        public IEnumerable<SelectListItem> Counties { get; set; }

        public bool IsRequired => !NonUK;
    }
}