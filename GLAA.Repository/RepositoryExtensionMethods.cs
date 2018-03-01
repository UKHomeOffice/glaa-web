using System.Collections.Generic;
using System.Linq;
using GLAA.Domain;

namespace GLAA.Repository
{
    public static class RepositoryExtensionMethods
    {
        public static IEnumerable<TEntity> FilterDeletedEntities<TEntity>(this IEnumerable<TEntity> entities,
            bool includeDeleted)
        {
            return typeof(IDeletable).IsAssignableFrom(typeof(TEntity))
                ? entities.Cast<IDeletable>().Where(x => !x.Deleted || includeDeleted).Cast<TEntity>()
                : entities;
        }
    }
}
