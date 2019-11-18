using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ST.DataAccess
{
    /// <summary>
    /// This interface is implemented by all repositories to ensure implementation of fixed methods.
    /// </summary>
    /// <typeparam name="TEntity">Main Entity type this repository works on</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        #region Select/Get/Query
        /// <summary>
        /// GetById
        /// </summary>
        /// <param name="id">primary key</param>
        /// <returns>Entity</returns>
        TEntity GetById(object id);

        /// <summary>
        /// GetByIdAsync
        /// </summary>
        /// <param name="id">primary key</param>
        /// <returns>Entity</returns>
        Task<TEntity> GetByIdAsync(object id);

        /// <summary>
        /// GetAll tracking, return IQueryable
        /// </summary>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// GetAll no tracking, return IQueryable
        /// </summary>
        IQueryable<TEntity> GetAllNoTracking();

        #endregion

        #region Insert
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="entity">Inserted entity</param>
        TEntity Insert(TEntity entity);

        /// <summary>
        /// Inserts
        /// </summary>
        /// <param name="entities">Inserted entity</param>
        void Insert(IEnumerable<TEntity> entities);

        /// <summary>
        /// Insert Async
        /// </summary>
        /// <param name="entity">Inserted entity</param>
        Task<TEntity> InsertAsync(TEntity entity);

        /// <summary>
        /// Inserts Async
        /// </summary>
        /// <param name="entities">Inserted entity</param>
        Task InsertAsync(IEnumerable<TEntity> entities);
        #endregion

        #region Update
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update(TEntity entity);

        /// <summary>
        /// Updates
        /// </summary>
        /// <param name="entities"></param>
        void Update(IEnumerable<TEntity> entities);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="properties">Action that can be used to change values of the entity</param>
        /// <returns>Updated entity</returns>
        void Update(TEntity entity, params Expression<Func<TEntity, object>>[] properties);
        #endregion

        #region Delete
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="entity">Entity to be deleted</param>
        void Delete(TEntity entity);


        /// <summary>
        /// Deletes
        /// </summary>
        /// <param name="entities"></param>
        void Delete(IEnumerable<TEntity> entities);


        /// <summary>
        /// Delete by lambda
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        void Delete(Expression<Func<TEntity, bool>> predicate);
        #endregion
    }
}
