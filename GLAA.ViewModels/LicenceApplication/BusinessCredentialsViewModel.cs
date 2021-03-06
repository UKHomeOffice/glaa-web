﻿using GLAA.Domain.Models;
using GLAA.ViewModels.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GLAA.ViewModels.LicenceApplication
{
    public class BusinessCredentialsViewModel : YesNoViewModel, IValidatable
    {
        public BusinessCredentialsViewModel()
        {
            CompaniesHouseRegistrationViewModel = new CompaniesHouseRegistrationViewModel();
        }

        public LegalStatusEnum LegalStatus { get; set; }

        public CompaniesHouseRegistrationViewModel CompaniesHouseRegistrationViewModel { get; set; }

        public VATStatusViewModel VATStatusViewModel { get; set; } = new VATStatusViewModel();

        public PAYEStatusViewModel PAYEStatusViewModel { get; set; } = new PAYEStatusViewModel();

        public TaxReferenceViewModel TaxReferenceViewModel { get; set; } = new TaxReferenceViewModel();

        public bool IsValid { get; set; }

        public void Validate()
        {
            VATStatusViewModel.Validate();
            PAYEStatusViewModel.Validate();
            TaxReferenceViewModel.Validate();

            IsValid = VATStatusViewModel.IsValid && PAYEStatusViewModel.IsValid && TaxReferenceViewModel.IsValid;

            if (LegalStatus == LegalStatusEnum.RegisteredCompany)
            {
                CompaniesHouseRegistrationViewModel.Validate();
                IsValid = IsValid && CompaniesHouseRegistrationViewModel.IsValid;
            }
        }
    }
}
