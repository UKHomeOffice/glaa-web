using System;
using System.Collections.Generic;
using GLAA.Domain;
using GLAA.Domain.Models;

namespace GLAA.Repository
{
    public interface IEntityFrameworkRepository
    {
        TEntity Create<TEntity>() where TEntity : class, IId, new();
        int Upsert<TEntity>(TEntity entity) where TEntity : class, IId;
        TEntity GetById<TEntity>(int id, bool includeDeleted = false) where TEntity : class, IId;
        TEntity Find<TEntity>(Func<TEntity, bool> predicate) where TEntity : class;
        IEnumerable<TEntity> GetAll<TEntity>(bool includeDeleted = false) where TEntity : class;
        /// <summary>
        /// Mark the entity with this ID as deleted and mark any of its properties 
        /// with the <see cref="CascadeDeleteAttribute"/> as deleted. This will only cascade down one "level".
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity to delete</typeparam>
        /// <param name="id">The ID of the entity to delete</param>
        void Delete<TEntity>(int id) where TEntity : class, IId, IDeletable;
    }
}