using System;
using System.Linq;
using GLAA.Common;
using GLAA.Domain.Models;
using GLAA.Repository;
using GLAA.ViewModels.Admin;

namespace GLAA.Services.Admin
{
    public class AdminLicencePostDataHandler : IAdminLicencePostDataHandler
    {
        private readonly IEntityFrameworkRepository repository;
        private readonly IDateTimeProvider dateTimeProvider;

        public AdminLicencePostDataHandler(IEntityFrameworkRepository repository, IDateTimeProvider dateTimeProvider)
        {
            this.repository = repository;
            this.dateTimeProvider = dateTimeProvider;
        }

        public void UpdateStatus(AdminLicenceViewModel model)
        {
            var statusChange = repository.Create<LicenceStatusChange>();
            statusChange.Licence = repository.GetById<Licence>(model.Licence.Id);
            statusChange.DateCreated = dateTimeProvider.Now();
            statusChange.Status = repository.GetById<LicenceStatus>(model.NewLicenceStatus);
            statusChange.Reason = repository.GetById<StatusReason>(model.NewStatusReason);
            statusChange.NonCompliantStandards = model.StandardCheckboxes.Where(x => x.Checked)
                .Select(y => repository.GetById<LicenceStatusChangeLicensingStandard>(y.Id)).ToList();

            repository.Upsert(statusChange);

            //Update the current status for the Licence.
            statusChange.Licence.CurrentStatusChange = statusChange;
            repository.Upsert(statusChange.Licence);
        }
    }
}
