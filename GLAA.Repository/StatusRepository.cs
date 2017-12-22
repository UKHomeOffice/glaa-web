using System;
using System.Collections.Generic;
using System.Linq;
using GLAA.Domain;
using GLAA.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GLAA.Repository
{
    public class StatusRepository : EntityFrameworkRepositoryBase, IStatusRepository
    {
        public StatusRepository(GLAAContext context) : base(context)
        {
        }

        public IEnumerable<LicenceStatus> GetNextStatusesForId(int id)
        {
            return Context.LicenceStatuses.Include(s => s.NextStatuses).Where(s => s.Id == id);
        }

        public LicenceStatus GetRandomStatus()
        {
            var rand = new Random();
            return Context.LicenceStatuses.ElementAt(rand.Next(Context.LicenceStatuses.Count()));
        }

        public LicenceStatus GetNewApplication()
        {
            return Context.LicenceStatuses.OrderBy(s => s.Id).First();
        }
    }
}
