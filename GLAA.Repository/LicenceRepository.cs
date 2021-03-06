﻿using GLAA.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using GLAA.Common;
using GLAA.Domain;
using Microsoft.EntityFrameworkCore;

namespace GLAA.Repository
{
    public class LicenceRepository : EntityFrameworkRepositoryBase, ILicenceRepository
    {
        public LicenceRepository(GLAAContext dbContext, IDateTimeProvider dtp) : base(dbContext, dtp)
        {
        }

        public Licence GetById(int id)
        {
            return GetAllEntriesWithStatusesAndAddress().FirstOrDefault(l => l.Id == id);
        }

        public Licence GetByApplicationId(string applicationId)
        {
            return GetAllEntriesWithStatusesAndAddress().FirstOrDefault(l =>
                l.ApplicationId.Equals(applicationId, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Licence> GetAllLicencesForPublicRegister()
        {
            return GetAllLicences().Where(x => GetLatestStatus(x).Status.ShowInPublicRegister);
        }

        public IEnumerable<Licence> GetAllLicences()
        {
            return GetAllEntriesWithStatusesAndAddress().Where(l =>
                l.LicenceStatusHistory.Any() &&
                l.LicenceStatusHistory.OrderByDescending(h => h.DateCreated).First().Status.IsLicence);
        }

        public IEnumerable<Licence> GetAllApplications()
        {
            return GetAllEntriesWithStatusesAndAddress().Where(l =>
                l.LicenceStatusHistory.Any() &&
                l.LicenceStatusHistory.OrderByDescending(h => h.DateCreated).First().Status.IsApplication);
        }

        public IQueryable<Licence> GetCoreLicenses()
        {
            return Context.Licences
                .Include(l => l.LicenceStatusHistory).ThenInclude(h => h.Status)
                .Include(l => l.LicenceStatusHistory).ThenInclude(h => h.NonCompliantStandards)
                .Include(l => l.LicenceStatusHistory).ThenInclude(h => h.Reason)
                .Include(l => l.Address)
                .Include(l => l.OperatingIndustries)
                .Include(l => l.OperatingCountries)
                .Include(l => l.SelectedSectors)
                .Include(l => l.SelectedMultiples)
                .Include(l => l.PreviousTradingNames)
                .Include(l => l.PAYENumbers)
                .Include(l => l.DirectorOrPartners).ThenInclude(x => x.CountryOfBirth)
                .Include(l => l.DirectorOrPartners).ThenInclude(x => x.Address).ThenInclude(a => a.Country)
                .Include(l => l.DirectorOrPartners).ThenInclude(x => x.RestraintOrders)
                .Include(l => l.DirectorOrPartners).ThenInclude(x => x.UnspentConvictions)
                .Include(l => l.DirectorOrPartners).ThenInclude(x => x.OffencesAwaitingTrial)
                .Include(l => l.PrincipalAuthorities).ThenInclude(x => x.CountryOfBirth)
                .Include(l => l.PrincipalAuthorities).ThenInclude(x => x.Address).ThenInclude(a => a.Country)
                .Include(l => l.PrincipalAuthorities).ThenInclude(x => x.RestraintOrders)
                .Include(l => l.PrincipalAuthorities).ThenInclude(x => x.UnspentConvictions)
                .Include(l => l.PrincipalAuthorities).ThenInclude(x => x.OffencesAwaitingTrial)
                .Include(l => l.AlternativeBusinessRepresentatives).ThenInclude(x => x.CountryOfBirth)
                .Include(l => l.AlternativeBusinessRepresentatives).ThenInclude(x => x.Address).ThenInclude(a => a.Country)
                .Include(l => l.AlternativeBusinessRepresentatives).ThenInclude(x => x.RestraintOrders)
                .Include(l => l.AlternativeBusinessRepresentatives).ThenInclude(x => x.UnspentConvictions)
                .Include(l => l.AlternativeBusinessRepresentatives).ThenInclude(x => x.OffencesAwaitingTrial)
                .Include(l => l.NamedIndividuals).ThenInclude(x => x.RestraintOrders)
                .Include(l => l.NamedIndividuals).ThenInclude(x => x.UnspentConvictions)
                .Include(l => l.NamedIndividuals).ThenInclude(x => x.OffencesAwaitingTrial);
            //.Include(l => l.LicenceStatusHistory).ThenInclude(c => c.Status).ThenInclude(s => s.NextStatuses).ThenInclude(n => n.NextStatus).ThenInclude(n => n.StatusReasons);
        }

        public IEnumerable<Licence> GetAllEntriesWithStatusesAndAddress()
        {
            return Context.Licences
                .Include(l => l.LicenceStatusHistory).ThenInclude(h => h.Status)
                .Include(l => l.LicenceStatusHistory).ThenInclude(h => h.NonCompliantStandards)
                .Include(l => l.LicenceStatusHistory).ThenInclude(h => h.Reason)
                .Include(l => l.Address)
                .Include(l => l.OperatingIndustries).ThenInclude(h => h.Industry)
                .Include(l => l.OperatingCountries)
                .Include(l => l.SelectedSectors).ThenInclude(h => h.Sector)
                .Include(l => l.SelectedMultiples)
                .Include(l => l.PreviousTradingNames)
                .Include(l => l.PAYENumbers)
                .Include(l => l.DirectorOrPartners).ThenInclude(x => x.CountryOfBirth)
                .Include(l => l.DirectorOrPartners).ThenInclude(x => x.Address).ThenInclude(a => a.Country)
                .Include(l => l.DirectorOrPartners).ThenInclude(x => x.RestraintOrders)
                .Include(l => l.DirectorOrPartners).ThenInclude(x => x.UnspentConvictions)
                .Include(l => l.DirectorOrPartners).ThenInclude(x => x.OffencesAwaitingTrial)
                .Include(l => l.PrincipalAuthorities).ThenInclude(x => x.CountryOfBirth)
                .Include(l => l.PrincipalAuthorities).ThenInclude(x => x.Address).ThenInclude(a => a.Country)
                .Include(l => l.PrincipalAuthorities).ThenInclude(x => x.RestraintOrders)
                .Include(l => l.PrincipalAuthorities).ThenInclude(x => x.UnspentConvictions)
                .Include(l => l.PrincipalAuthorities).ThenInclude(x => x.OffencesAwaitingTrial)
                .Include(l => l.AlternativeBusinessRepresentatives).ThenInclude(x => x.CountryOfBirth)
                .Include(l => l.AlternativeBusinessRepresentatives).ThenInclude(x => x.Address).ThenInclude(a => a.Country)
                .Include(l => l.AlternativeBusinessRepresentatives).ThenInclude(x => x.RestraintOrders)
                .Include(l => l.AlternativeBusinessRepresentatives).ThenInclude(x => x.UnspentConvictions)
                .Include(l => l.AlternativeBusinessRepresentatives).ThenInclude(x => x.OffencesAwaitingTrial)
                .Include(l => l.NamedIndividuals).ThenInclude(x => x.RestraintOrders)
                .Include(l => l.NamedIndividuals).ThenInclude(x => x.UnspentConvictions)
                .Include(l => l.NamedIndividuals).ThenInclude(x => x.OffencesAwaitingTrial)
                .Include(l => l.NamedJobTitles)
                .Include(l => l.LicenceStatusHistory).ThenInclude(c => c.Status).ThenInclude(s => s.NextStatuses).ThenInclude(n => n.NextStatus).ThenInclude(n => n.StatusReasons)
                .Include(l => l.CurrentStatusChange)
                .Include(l => l.CurrentSubmittedStatusChange)
                .Include(l => l.CurrentCommencementStatusChange);
        }

        public static LicenceStatusChange GetLatestStatus(Licence licence)
        {
            return licence.LicenceStatusHistory.OrderByDescending(h => h.DateCreated).First();
        }

        public static LicenceStatusChange GetLatestLicenceSubmissionStatus(Licence licence)
        {
            return licence.LicenceStatusHistory.OrderByDescending(h => h.DateCreated).FirstOrDefault(x => x.Status.LicenceSubmitted);
        }

        public static LicenceStatusChange GetLatestLicenceIssueStatus(Licence licence)
        {
            return licence.LicenceStatusHistory.OrderByDescending(h => h.DateCreated).FirstOrDefault(x => x.Status.LicenceIssued);
        }
    }
}
