using System;
using GLAA.Domain;
using GLAA.Domain.Models;

namespace GLAA.Repository
{
    public class RoleRepository : EntityFrameworkRepositoryBase, IRoleRepository
    {
        public RoleRepository(GLAAContext context) : base(context)
        {
        }

        public RoleDescription GetByName(string name)
        {
            return Find<RoleDescription>(r => r.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
