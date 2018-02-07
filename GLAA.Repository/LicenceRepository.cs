using GLAA.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using GLAA.Domain;
using Microsoft.EntityFrameworkCore;

namespace GLAA.Repository
{
    public class LicenceRepository : EntityFrameworkRepositoryBase, ILicenceRepository
    {
        public LicenceRepository(GLAAContext dbContext) : base(dbContext)
        {
        }

        public Licence GetById(int id)
        {
            return GetAllEntriesWithStatusesAndAddress().First(l => l.Id == id);
        }

        public Licence GetByApplicationId(string applicationId)
        {
            return GetAllEntriesWithStatusesAndAddress().First(l => l.ApplicationId.Equals(applicationId, StringComparison.OrdinalIgnoreCase));            
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

        public IEnumerable<Licence> GetAllEntriesWithStatusesAndAddress()
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
                .Include(l => l.DirectorOrPartners).ThenInclude(x => x.Address)
                .Include(l => l.DirectorOrPartners).ThenInclude(x => x.RestraintOrders)
                .Include(l => l.DirectorOrPartners).ThenInclude(x => x.UnspentConvictions)
                .Include(l => l.DirectorOrPartners).ThenInclude(x => x.OffencesAwaitingTrial)
                .Include(l => l.PrincipalAuthorities).ThenInclude(x => x.Address)
                .Include(l => l.PrincipalAuthorities).ThenInclude(x => x.RestraintOrders)
                .Include(l => l.PrincipalAuthorities).ThenInclude(x => x.UnspentConvictions)
                .Include(l => l.PrincipalAuthorities).ThenInclude(x => x.OffencesAwaitingTrial)
                .Include(l => l.AlternativeBusinessRepresentatives).ThenInclude(x => x.Address)
                .Include(l => l.AlternativeBusinessRepresentatives).ThenInclude(x => x.RestraintOrders)
                .Include(l => l.AlternativeBusinessRepresentatives).ThenInclude(x => x.UnspentConvictions)
                .Include(l => l.AlternativeBusinessRepresentatives).ThenInclude(x => x.OffencesAwaitingTrial)
                .Include(l => l.NamedIndividuals).ThenInclude(x => x.RestraintOrders)
                .Include(l => l.NamedIndividuals).ThenInclude(x => x.UnspentConvictions)
                .Include(l => l.NamedIndividuals).ThenInclude(x => x.OffencesAwaitingTrial)
                .Include(l => l.LicenceStatusHistory).ThenInclude(c => c.Status).ThenInclude(s => s.NextStatuses).ThenInclude(n => n.NextStatus).ThenInclude(n => n.StatusReasons);
        }

        public LicenceStatusChange GetLatestStatus(Licence licence)
        {
            return licence.LicenceStatusHistory.OrderByDescending(h => h.DateCreated).First();
        }
    }
}
