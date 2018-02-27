using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GLAA.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.ViewModels.LicenceApplication
{
    public class NamedIndividualCollectionViewModel : IValidatable, IIsSubmitted
    {
        public string IntroText
        {
            get
            {
                var bandA = "Your fee band is 'A'. For applications to supply workers into Agriculture, Horticulture, Food Processing and Packaging sectors only in Turnover Bands A and B may choose between entering named individuals or employee counts by job title.Select an input option";

                var bandB =
                    "Your fee band is 'B'. For applications to supply workers into Agriculture, Horticulture, Food Processing and Packaging sectors only in Turnover Bands A and B may choose between entering named individuals or employee counts by job title.Select an input option";

                var bandC = "Your fee band is 'C'. You may enter the details of named individuals.";

                var bandD = "Your fee band is 'D'. You may enter the details of named individuals.";
                var shellfish =
                    "You intend to operate in the shellfish gathering sector. We require the details of anyone who is authorised to act on your behalf.  You may enter the details of named individuals.";

                if (IsShellfish)
                {
                    return shellfish;
                }

                switch (TurnoverBand)
                {
                    case TurnoverBand.OverTenMillion:
                        return bandA;
                    case TurnoverBand.FiveToTenMillion:
                        return bandB;
                    case TurnoverBand.OneToFiveMillion:
                        return bandC;
                    case TurnoverBand.UnderOneMillion:
                        return bandD;
                    default:
                        return "You have not yet selected your turnover band or industry type(s). " +
                               "You can continue to add named individuals but you may be able to save time by selecting your industry and turnover before completing this section";
                }
            }
        }

        public TurnoverBand TurnoverBand { get; set; }
        public bool IsShellfish { get; set; }

        [Display(Name = "Job Titles / Named Individuals")]
        public NamedIndividualType NamedIndividualType { get; set; } = NamedIndividualType.PersonalDetails;
        public List<SelectListItem> AvailableIndividualTypes { get; set; } = new List<SelectListItem>
        {
            new SelectListItem
            {
                Value = "PersonalDetails",
                Text = "Personal Details"
            },
            new SelectListItem
            {
                Value = "JobTitles",
                Text = "Job Titles"
            }
        };

        public IEnumerable<NamedIndividualViewModel> NamedIndividuals { get; set; }
        public IEnumerable<NamedJobTitleViewModel> NamedJobTitles { get; set; }

        public NamedIndividualCollectionViewModel()
        {
            NamedIndividuals = new List<NamedIndividualViewModel>();
            NamedJobTitles = new List<NamedJobTitleViewModel>();
        }


        public void Validate()
        {
            if (NamedIndividualType == NamedIndividualType.PersonalDetails)
            {
                foreach (var ni in NamedIndividuals)
                {
                    ni.Validate();
                }
                IsValid = NamedIndividuals.Any() && NamedIndividuals.All(ni => ni.IsValid);
            }

            if (NamedIndividualType == NamedIndividualType.JobTitles)
            {
                foreach (var njt in NamedJobTitles)
                {
                    njt.Validate();
                }
                IsValid = NamedJobTitles.Any() && NamedJobTitles.All(ni => ni.IsValid);
            }
        }

        public bool IsValid { get; set; }

        public bool IsSubmitted { get; set; }
    }
}