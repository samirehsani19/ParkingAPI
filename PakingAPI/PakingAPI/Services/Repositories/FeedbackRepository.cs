using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PakingAPI.DTO;
using PakingAPI.Models;
using PakingAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace PakingAPI.Services.Repositories
{
    public class FeedbackRepository : Repository, IFeedbackRepository
    {
        public FeedbackRepository(DataContext context, ILogger<FeedbackRepository> logger) : base(context, logger)
        { }
        public virtual async Task<Feedback[]> GetAllFeedbacks(bool includeParkings, bool includeUsers)
        {
            _logger.LogInformation($"Getting feedbacks");
            IQueryable<Feedback> query = _dbContext.Feedbacks;
            if (includeUsers)
            {
                query = query.Include(x => x.User);
            }
            if (includeParkings)
            {
                query = query.Include(x => x.Parking);
            }
            return await query.ToArrayAsync();
        }

        public virtual async Task<Feedback> GetFeedbackByID(int id, bool includeParking, bool includeUser)
        {
            _logger.LogInformation($"Getting feedback by id: {id}");
            IQueryable<Feedback> query = _dbContext.Feedbacks.Where(x=> x.FeedbackID==id);
            if (includeParking)
            {
                query = query.Include(x => x.Parking);
            }
            if (includeUser)
            {
                query = query.Include(c => c.User);
            }

            if (query == null) throw new System.Exception($"Feedback with id: {id} could not found");
            return await query.SingleOrDefaultAsync();
        }
    }
}
