using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GLAA.ViewModels.LicenceApplication
{
    public class DirectorOrPartnerViewModel : PersonViewModel, IValidatable
    {
        public DirectorOrPartnerViewModel()
        {
            IsPreviousPrincipalAuthority = new IsPreviousPrincipalAuthorityViewModel();
            RightToWorkViewModel = new RightToWorkViewModel();
        }

        public IsPreviousPrincipalAuthorityViewModel IsPreviousPrincipalAuthority { get; set; }

        public int? PrincipalAuthorityId { get; set; }

        public void Validate()
        {
            var invalidModelFields = new List<string>();
            foreach (var prop in GetType().GetProperties())
            {
                var obj = prop.GetValue(this) ?? string.Empty;

                var validatable = obj as IValidatable;

                bool propertyIsValid;

                if (validatable != null)
                {
                    // Use the defined validate method if one is defined
                    validatable.Validate();
                    propertyIsValid = validatable.IsValid;
                }
                else
                {
                    // Use the validation context for properties
                    var context = new ValidationContext(obj, null);
                    propertyIsValid = Validator.TryValidateObject(obj, context, null, true);
                }

                if (!propertyIsValid)
                {
                    invalidModelFields.Add(prop.Name);
                }
            }
            IsValid = !invalidModelFields.Any();
        }

        public bool IsValid { get; set; }

        public int? Id { get; set; }

        public RightToWorkViewModel RightToWorkViewModel { get; set; }
    }

    public class IsPreviousPrincipalAuthorityViewModel : YesNoViewModel
    {
        [Required]
        [Display(Name = "Principal Authority?")]
        public bool? IsPreviousPrincipalAuthority { get; set; }
    }

    public class DirectorOrPartnerCollectionViewModel : IValidatable
    {
        [Required]
        [Display(Name = "Number of Directors or Partners")]
        public int? NumberOfDirectorsOrPartners { get; set; }

        public IEnumerable<DirectorOrPartnerViewModel> DirectorsOrPartners { get; set; }

        public bool DirectorsRequired { get; set; }

        public DirectorOrPartnerCollectionViewModel()
        {
            DirectorsOrPartners = new List<DirectorOrPartnerViewModel>();
        }

        public void Validate()
        {
            if (!NumberOfDirectorsOrPartners.HasValue)
            {
                IsValid = false;
                return;
            }

            if (DirectorsRequired && (!DirectorsOrPartners.Any() || (NumberOfDirectorsOrPartners ?? 0) < 1))
            {
                IsValid = false;
                return;
            }

            foreach (var dop in DirectorsOrPartners)
            {
                dop.Validate();
            }
            IsValid = DirectorsOrPartners.Count() == NumberOfDirectorsOrPartners.Value &&
                      DirectorsOrPartners.Count(d =>
                          d.IsPreviousPrincipalAuthority.IsPreviousPrincipalAuthority.HasValue &&
                          d.IsPreviousPrincipalAuthority.IsPreviousPrincipalAuthority.Value) <= 1 &&
                      DirectorsOrPartners.All(abr => abr.IsValid);
        }

        public bool IsValid { get; set; }
    }
}