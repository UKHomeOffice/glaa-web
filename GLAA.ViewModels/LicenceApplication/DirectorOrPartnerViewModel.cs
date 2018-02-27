using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GLAA.ViewModels.LicenceApplication
{
    public class DirectorOrPartnerViewModel : PersonViewModel
    {
        public DirectorOrPartnerViewModel()
        {
            IsPreviousPrincipalAuthority = new IsPreviousPrincipalAuthorityViewModel();
            RightToWorkViewModel = new RightToWorkViewModel();
        }

        public IsPreviousPrincipalAuthorityViewModel IsPreviousPrincipalAuthority { get; set; }

        public int? PrincipalAuthorityId { get; set; }

        public int? Id { get; set; }

        public RightToWorkViewModel RightToWorkViewModel { get; set; }
        public bool HasPrincipalAuthoritySelected { get; set; }
    }

    public class IsPreviousPrincipalAuthorityViewModel : YesNoViewModel, ICanView<DirectorOrPartnerViewModel>
    {
        [Required]
        [Display(Name = "Principal Authority?")]
        public bool? IsPreviousPrincipalAuthority { get; set; }

        public bool CanView(DirectorOrPartnerViewModel parent)
        {
            return !parent.HasPrincipalAuthoritySelected;
        }
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