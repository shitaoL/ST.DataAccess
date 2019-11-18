using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace ST.DataAccess
{
    /// <summary>
    /// Defines the interface(s) for unit of work.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        int SaveChanges();

        /// <summary>
        /// Asynchronously saves all changes made in this unit of work to the database.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous save operation. The task result contains the number of state entities written to database.</returns>
        Task<int> SaveChangesAsync();

        #region command sql
        /// <summary>
        /// QueryAsync
        /// ag:await _unitOfWork.QueryAsync("select id,name from table where id = @id", new { id = 1 }); [please replace the @id to :id for oracle]
        /// </summary>
        /// <typeparam name="TEntity">entity</typeparam>
        /// <param name="sql">sql string</param>
        /// <param name="param">parameter</param>
        /// <param name="trans">transaction</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> QueryAsync<TEntity>(string sql, object param = null, IDbContextTransaction trans = null) where TEntity : class;

        /// <summary>
        /// ExecuteAsync
        /// ag:await _unitOfWork.ExecuteAsync("update table set name =@name where id =@id", new { name = "", id=1 }); [please replace the @id to :id for oracle]
        /// </summary>
        /// <param name="sql">sql string</param>
        /// <param name="param">parameter</param>
        /// <param name="trans">transaction</param>
        /// <returns></returns>
        Task<int> ExecuteAsync(string sql, object param, IDbContextTransaction trans = null);
       
        #endregion


        /// <summary>
        /// BeginTransaction
        /// </summary>
        /// <returns></returns>
        IDbContextTransaction BeginTransaction();

        /// <summary>
        /// get connection
        /// </summary>
        /// <returns></returns>
        IDbConnection GetConnection();
    }
}
