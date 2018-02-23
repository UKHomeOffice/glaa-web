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
        TEntity GetById<TEntity>(int id) where TEntity : class, IId;
        TEntity Find<TEntity>(Func<TEntity, bool> predicate) where TEntity : class;
        IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class;
        void Delete<TEntity>(int id) where TEntity : class, IId, IDeletable;
    }
}