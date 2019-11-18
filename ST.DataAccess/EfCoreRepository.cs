using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ST.DataAccess
{
    /// <summary>
    /// EfCoreRepository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EfCoreRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        private DbSet<TEntity> _dbSet;
        public EfCoreRepository(DbContext context)
        {
            _context = context;
        }

        protected virtual DbSet<TEntity> DbSet => _dbSet ?? (_dbSet = _context.Set<TEntity>());

        public TEntity GetById(object id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            return DbSet.Find(id);
        }
        public async Task<TEntity> GetByIdAsync(object id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            return await DbSet.FindAsync(id);
        }
        public virtual IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }
        public virtual IQueryable<TEntity> GetAllNoTracking()
        {
            return DbSet.AsNoTracking();
        }


        public TEntity Insert(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            return DbSet.Add(entity).Entity;
        }
        public void Insert(IEnumerable<TEntity> entities)
        {
            if (!entities.Any())
                throw new ArgumentNullException("entities");

            DbSet.AddRange(entities);
        }
        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            return (await DbSet.AddAsync(entity)).Entity;
        }
        public async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            if (!entities.Any())
                throw new ArgumentNullException("entities");

            await DbSet.AddRangeAsync(entities);
        }


        public void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            DbSet.Attach(entity);
            _context.Update(entity);
        }
        public void Update(IEnumerable<TEntity> entities)
        {
            if (!entities.Any())
                throw new ArgumentNullException("entities");

            _context.UpdateRange(entities);

        }
        public void Update(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                if (string.IsNullOrEmpty(propertyName))
                {
                    propertyName = GetPropertyName(property.Body.ToString());
                }
                _context.Entry(entity).Property(propertyName).IsModified = true;

            }
        }


        public void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _context.Remove(entity);
        }
        public void Delete(IEnumerable<TEntity> entities)
        {
            if (!entities.Any())
                throw new ArgumentNullException("entities");

            _context.RemoveRange(entities);

        }
        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");
            _context.RemoveRange(DbSet.Where(predicate));
        }

        #region private      
        private string GetPropertyName(string str)
        {
            return str.Split(',')[0].Split('.')[1];
        }
        private void AttachIfNot(TEntity entity)
        {
            var entry = _context.ChangeTracker.Entries().FirstOrDefault(ent => ent.Entity == entity);
            if (entry != null)
            {
                return;
            }
            _context.Attach(entity);
        }

        #endregion


    }
}
