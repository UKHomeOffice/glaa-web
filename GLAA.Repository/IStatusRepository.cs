using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLAA.Domain.Models;

namespace GLAA.Repository
{
    public interface IStatusRepository : IEntityFrameworkRepository
    {
        IEnumerable<LicenceStatus> GetNextStatusesForId(int id);
        LicenceStatus GetRandomStatus();
        LicenceStatus GetNewApplication();
    }
}
