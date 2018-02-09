using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GLAA.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.ViewModels.LicenceApplication
{
    public class AddressViewModel : IId, INeedCountries, INeedCounties
    {
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
    }
}