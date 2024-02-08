using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Application.Interfaces;
using ToDoList.Infrastructure.Persistence;

namespace ToDoList.Infrastructure.Services
{
    public class GenericRepository : IGenericRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public GenericRepository(ApplicationDbContext dbContext)
        {
                _dbContext = dbContext;
        }
        public async Task Delete<TEntity>(object id) where TEntity : class
        {
            try
            {
                var entityToDelete = await _dbContext.Set<TEntity>().FindAsync(id);

                if (entityToDelete == null) throw new ArgumentNullException("Entity");

                _dbContext.Set<TEntity>().Remove(entityToDelete);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> Exists<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class
        {
            try
            {
                var obj = await _dbContext.Set<TEntity>().AnyAsync(filter);
                if(obj == null) return false;
                return true;

            }catch(Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<TEntity>> Get<TEntity>() where TEntity : class
        {
            try
            {
                return await _dbContext.Set<TEntity>().ToListAsync();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<TEntity> GetById<TEntity>(object id) where TEntity : class
        {
            try
            {
                return await _dbContext.Set<TEntity>().FindAsync(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TEntity> GetFirstOrDefault<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            try
            {
                return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Insert<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                await _dbContext.Set<TEntity>().AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                var entityType = typeof(TEntity);
                var idProperty = entityType.GetProperty("Id");

                if (idProperty != null)
                {
                    return (int)idProperty.GetValue(entity);
                }
                else
                {
                    throw new InvalidOperationException("Entity does not have an ID property.");
                }


            }
            catch (Exception ex)
            {
                throw; 
            }
        }



        public async Task Update<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
