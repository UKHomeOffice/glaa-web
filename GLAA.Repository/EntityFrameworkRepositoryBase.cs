﻿using GLAA.Domain;
using GLAA.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            var entity = Context.Set<TEntity>().Find(id);

            if (entity is IDeletable deletable)
            {
                return deletable.Deleted ? null : entity;
            }

            return entity;
        }

        public TEntity GetDeletedById<TEntity>(int id) where TEntity : class, IId, IDeletable
        {
            return Context.Set<TEntity>().Find(id);
        }

        public TEntity Find<TEntity>(Func<TEntity, bool> predicate) where TEntity : class
        {
            return Context.Set<TEntity>().FirstOrDefault(predicate);
        }

        public IEnumerable<TEntity> GetAll<TEntity>(bool includeDeleted = false) where TEntity : class
        {
            return Context.Set<TEntity>().FilterDeletedEntities(includeDeleted).ToList();
        }

        public void Delete<TEntity>(int id) where TEntity : class, IId, IDeletable
        {
            var entity = GetById<TEntity>(id);

            var now = DateTimeProvider.Now();

            CascadeDelete(entity, now);

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

        private void CascadeDelete<TEntity>(TEntity entity, DateTime now) where TEntity : class, IDeletable
        {
            if (entity == null)
            {
                return;
            }

            // Delete the parent entity
            entity.Deleted = true;
            entity.DateDeleted = now;

            // Find all child references which need to be deleted
            var propsToCascade = entity.GetType().GetProperties()
                .Where(f => f.GetCustomAttributes(typeof(CascadeDeleteAttribute), false).Any());

            foreach (var prop in propsToCascade)
            {
                // Are we deleting a single item or a collection?
                if (typeof(IDeletable).IsAssignableFrom(prop.PropertyType))
                {
                    // Build an expression to access the value of this property
                    var parameterExpression = Expression.Parameter(entity.GetType(), "parent");
                    var propertyAccessExpression = Expression.Property(parameterExpression, prop);
                    var propertyAccessLambda = Expression.Lambda<Func<TEntity, IDeletable>>(propertyAccessExpression, parameterExpression);

                    // Explicitly load the reference from the DB
                    Context.Entry(entity).Reference(propertyAccessLambda).Load();
                    
                    // Get the reference object from the parent
                    var deletable = propertyAccessLambda.Compile()(entity);

                    // Set the reference as deleted
                    deletable.Deleted = true;
                    deletable.DateDeleted = now;
                }
                else if (typeof(IEnumerable<IDeletable>).IsAssignableFrom(prop.PropertyType))
                {
                    // Build an expression to access the value of this property
                    var parameterExpression = Expression.Parameter(entity.GetType(), "parent");
                    var propertyAccessExpression = Expression.Property(parameterExpression, prop);
                    var propertyAccessLambda = Expression.Lambda<Func<TEntity, IEnumerable<IDeletable>>>(propertyAccessExpression, parameterExpression);

                    // Explicitly load the reference collection from the DB
                    Context.Entry(entity).Collection(propertyAccessLambda).Load();

                    // Get the reference collection from the parent
                    var deletableCollection = propertyAccessLambda.Compile()(entity);

                    // Set each reference as deleted
                    foreach (var deletable in deletableCollection)
                    {
                        deletable.Deleted = true;
                        deletable.DateDeleted = now;
                    }
                }
            }
        }
    }
}
