using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PakingAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PakingAPI.Services
{
    public class ParkingRepository : Repository, IParkingRepo
    {
        public ParkingRepository(DataContext context, ILogger<ParkingRepository> logger):base(context, logger)
        {

        }
        public virtual async Task<Parking[]> GetAllParking(bool includeFeedback, bool includeUser)
        {
            _logger.LogInformation($"Getting all parkings");
            IQueryable<Parking> query = _dbContext.Parkings;
            if (includeFeedback)
            {
                query = query.Include(x => x.feedbacks);
            }
            if (includeUser)
            {
                query = query.Include(x => x.User);
            }
            return await query.ToArrayAsync();
        }

        public virtual async Task<Parking> GetParkingById(int id, bool includeFeedback, bool includeUser)
        {
            _logger.LogInformation($"Getting parking by id: {id}");
            IQueryable<Parking> query = _dbContext.Parkings.Where(x => x.ParkingID == id);
            if (includeFeedback)
            {
                query = query.Include(x => x.feedbacks);
            }
            if (includeUser)
            {
                query = query.Include(x => x.User);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Parking> GetParkingByStreet(string streedName, bool includeFeedback, bool includeUser)
        {
            _logger.LogInformation($"Getting parkings by street name : {streedName}");
            IQueryable<Parking> query = _dbContext.Parkings.Where(x => x.StreetAdress == streedName);
            if (includeFeedback)
            {
                query = query.Include(x => x.feedbacks);
            }
            if (includeUser)
            {
                query = query.Include(x => x.User);
            }
            return await query.SingleOrDefaultAsync();
        }
    }
}
