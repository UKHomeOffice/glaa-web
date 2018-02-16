using GLAA.Domain.Models;
using System.Collections.Generic;

namespace GLAA.Repository
{
    public interface ILicenceRepository : IEntityFrameworkRepository
    {
        Licence GetById(int id);
        Licence GetByApplicationId(string applicationId);
        IEnumerable<Licence> GetAllLicencesForPublicRegister();
        IEnumerable<Licence> GetAllLicences();
        IEnumerable<Licence> GetAllApplications();
        IEnumerable<Licence> GetAllEntriesWithStatusesAndAddress();
    }
}
