using GLAA.Domain;
using GLAA.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using GLAA.Common;

namespace GLAA.Repository
{
    public class EntityFrameworkRepositoryBase : IEntityFrameworkRepository
    {
        protected readonly GLAAContext Context;
        protected readonly IDateTimeProvider DateTimeProvider;

        public EntityFrameworkRepositoryBase(GLAAContext context, IDateTimeProvider dtp)
        {
            Context = context;
            DateTimeProvider = dtp;
        }

        public TEntity GetById<TEntity>(int id) where TEntity : class, IId
        {
            return Context.Set<TEntity>().Find(id);            
        }

        public TEntity Find<TEntity>(Func<TEntity, bool> predicate) where TEntity : class
        {
            return Context.Set<TEntity>().FirstOrDefault(predicate);
        }

        public IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return Context.Set<TEntity>().ToList();
        }

        public void Delete<TEntity>(int id) where TEntity : class, IId, IDeletable
        {
            var entity = GetById<TEntity>(id);
            entity.Deleted = true;
            entity.DateDeleted = DateTimeProvider.Now();
            Context.SaveChanges();
        }

        public TEntity Create<TEntity>() where TEntity : class, IId, new()
        {
            //var entity = Context.Set<TEntity>().
            var entity = new TEntity();
            Context.Set<TEntity>().Attach(entity);            
            return entity;
        }

        public int Upsert<TEntity>(TEntity entity) where TEntity : class, IId
        {
            Context.SaveChanges();
            return entity.Id;
        }
    }
}
