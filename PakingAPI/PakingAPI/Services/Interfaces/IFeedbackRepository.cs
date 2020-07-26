using PakingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PakingAPI.Services.Interfaces
{
    public interface IFeedbackRepository:IRepository
    {
        Task<Feedback[]> GetAllFeedbacks(bool includeParkings, bool IncludeUsers);
        Task<Feedback> GetFeedbackByID(int id, bool includeParking, bool includeUser);
    }
}
