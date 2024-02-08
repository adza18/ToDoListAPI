using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Application.Interfaces
{
    public interface IGenericRepository
    {
        Task<bool> Exists<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class;

        Task<TEntity> GetById<TEntity>(object id) where TEntity : class;
        Task<List<TEntity>> Get<TEntity>() where TEntity : class;


        Task<TEntity> GetFirstOrDefault<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class;

        Task<int> Insert<TEntity>(TEntity entity) where TEntity : class;

        Task Update<TEntity>(TEntity  entity) where TEntity
            : class;
        Task Delete<TEntity>(object id) where TEntity : class;

    }
}
