using GLAA.Domain.Models;
using System.Collections.Generic;

namespace GLAA.Repository
{
    public interface ILicenceRepository : IEntityFrameworkRepository
    {
        Licence GetById(int id, bool includeDeleted = false);
        Licence GetByApplicationId(string applicationId, bool includeDeleted = false);
        IEnumerable<Licence> GetAllLicencesForPublicRegister();
        IEnumerable<Licence> GetAllLicences(bool includeDeleted = false);
        IEnumerable<Licence> GetAllApplications(bool includeDeleted = false);
        IEnumerable<Licence> GetAllEntriesWithStatusesAndAddress(bool includeDeleted = false);
    }
}
