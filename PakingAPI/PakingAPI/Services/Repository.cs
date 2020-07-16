using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PakingAPI.Services
{
    public class Repository : IRepository
    {
        protected readonly DataContext _dbContext;
        protected readonly ILogger<Repository> _logger;
        public Repository(DataContext context, ILogger<Repository> logger)
        {
            this._dbContext = context;
            this._logger = logger;
        }
        public virtual void Add<T>(T entity) where T : class
        {
            _logger.LogInformation($"Adding object of type {entity.GetType()}");
            _dbContext.Add(entity);
        }

        public virtual void Delete<T>(T entity) where T : class
        {
            _logger.LogInformation($"Deleting object of type {entity.GetType()}");
            _dbContext.Remove(entity);
        }

        public virtual async Task<bool> Save()
        {
            _logger.LogInformation($"Saving changes");
            return (await _dbContext.SaveChangesAsync()) >= 0;
        }

        public virtual void Update<T>(T entity) where T : class
        {
            _logger.LogInformation($"Updating object of type {entity.GetType()}");
            _dbContext.Update(entity);
        }
    }
}
