using CatechistHelper.Domain.Common;
using CatechistHelper.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Infrastructure.Repositories
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        public TContext Context { get; }
        private Dictionary<Type, object> _repositories;

        public UnitOfWork(TContext context)
        {
            Context = context;
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            _repositories ??= new Dictionary<Type, object>();
            if (_repositories.TryGetValue(typeof(TEntity), out object repository))
            {
                return (IGenericRepository<TEntity>)repository;
            }

            repository = new GenericRepository<TEntity>(Context);
            _repositories.Add(typeof(TEntity), repository);
            return (IGenericRepository<TEntity>)repository;
        }

        public void Dispose()
        {
            Context?.Dispose();
        }

        public int Commit()
        {
            TrackChanges();
            UpdateEntites();
            return Context.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            TrackChanges();
            UpdateEntites();
            return await Context.SaveChangesAsync();
        }

        private void TrackChanges()
        {
            var validationErrors = Context.ChangeTracker.Entries<IValidatableObject>()
                .SelectMany(e => e.Entity.Validate(null))
                .Where(e => e != ValidationResult.Success)
                .ToArray();
            if (validationErrors.Any())
            {
                var exceptionMessage = string.Join(Environment.NewLine,
                    validationErrors.Select(error => $"Properties {error.MemberNames} Error: {error.ErrorMessage}"));
                throw new Exception(exceptionMessage);
            }
        }

        private void UpdateEntites()
        {
            IEnumerable<EntityEntry<BaseEntity>> entries = Context.ChangeTracker.Entries<BaseEntity>();

            foreach (EntityEntry<BaseEntity> entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property(x => x.CreatedAt)
                        .CurrentValue = DateTime.UtcNow;
                }

                if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Property(x => x.UpdatedAt)
                        .CurrentValue = DateTime.UtcNow;
                }
            }
        }
    }
}
