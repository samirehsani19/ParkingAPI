using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PakingAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PakingAPI.Services
{
    public class UserRepository:Repository, IUserRepo
    {
        public UserRepository(DataContext context, ILogger<UserRepository> logger) : base(context, logger)
        {

        }

        public virtual async Task<User[]> GetAllUser(bool includeParking, bool includeFeedback)
        {
            _logger.LogInformation($"Getting all users");
            IQueryable<User> query = _dbContext.Users;

            if (includeParking)
            {
                query = query.Include(x => x.Parkings);
            }
            if (includeFeedback)
            {
                query = query.Include(x => x.Feedbacks);
            }
            return await query.ToArrayAsync();
        }

        public virtual async Task<User> GetUserByID(int id, bool includeParking=false, bool includeFeeback=false)
        {
            _logger.LogInformation($"Getting user by id: {id}");
            IQueryable<User> query = _dbContext.Users.Where(x=> x.UserID==id);
            if (includeFeeback)
            {
                query = query.Include(x => x.Feedbacks);
            }
            if (includeParking)
            {
                query = query.Include(x => x.Parkings);
            }
            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<User> GetUserByName(string name,bool includeparking, bool includeFeedback)
        {
            _logger.LogInformation($"Getting user by name: {name}");
            IQueryable<User> query = _dbContext.Users.Where(x => x.FirstName == name);
            if (includeFeedback)
            {
                query = query.Include(x => x.Feedbacks);
            }
            if (includeparking)
            {
                query = query.Include(x => x.Parkings);
            }
            return await query.FirstOrDefaultAsync();
        }
    }
}
