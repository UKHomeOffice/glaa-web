using System;
using System.Collections.Generic;
using GLAA.Domain.Models;
using GLAA.ViewModels;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.LicenceApplication
{
    public interface ILicenceApplicationViewModelBuilder : IViewModelBuilder<LicenceApplicationViewModel>
    {
        LicenceApplicationViewModel Build(string applicationId);

        LicenceApplicationViewModel Build(int id);

        IList<LicenceApplicationViewModel> BuildLicencesForUser(string userId);

        T Build<T>(int licenceId) where T : new();

        T Build<T, U>(int licenceId, Func<Licence, U> objectSelector) where T : new() where U : new();

        T Build<T, U>(int licenceId, Func<Licence, ICollection<U>> objectSelector) where T : new();

        T BuildCountriesFor<T>(T model) where T : INeedCountries;
    }
}
