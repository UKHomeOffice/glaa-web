using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GLAA.ViewModels.LicenceApplication
{
    public class AlternativeBusinessRepresentativeCollectionViewModel : YesNoViewModel, IValidatable
    {
        public IEnumerable<AlternativeBusinessRepresentativeViewModel> AlternativeBusinessRepresentatives { get; set; }

        [Display(Name = "Do you wish to add any Alternative Business Representatives?")]
        public bool? HasAlternativeBusinessRepresentatives { get; set; }

        public AlternativeBusinessRepresentativeCollectionViewModel()
        {
            AlternativeBusinessRepresentatives = new List<AlternativeBusinessRepresentativeViewModel>();
        }

        public void Validate()
        {
            if (HasAlternativeBusinessRepresentatives == null)
            {
                IsValid = false;
                return;
            }
            var hasNoAbrs = !HasAlternativeBusinessRepresentatives.Value;
            if (hasNoAbrs)
            {
                IsValid = true;
                return;
            }

            foreach (var abr in AlternativeBusinessRepresentatives)
            {                
                abr.Validate();
            }
            IsValid = AlternativeBusinessRepresentatives.Any() &&
                      AlternativeBusinessRepresentatives.Count() <= 2 &&
                      AlternativeBusinessRepresentatives.All(abr => abr.IsValid);
        }

        public bool IsValid { get; set; }
    }
}