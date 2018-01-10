using System;
using System.Collections.Generic;
using System.Text;
using GLAA.Domain.Models;

namespace GLAA.Repository
{
    public interface IRoleRepository : IEntityFrameworkRepository
    {
        RoleDescription GetByName(string name);
    }
}
