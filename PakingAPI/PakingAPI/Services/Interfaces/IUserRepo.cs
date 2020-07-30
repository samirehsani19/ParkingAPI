using PakingAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PakingAPI.Services
{
    public interface IUserRepo:IRepository
    {
        Task<User[]> GetAllUser(bool includeParking, bool includeFeedback);
        Task<User> GetUserByID(int id, bool includeParking=false, bool includeFeeback=false);
        Task<User> GetUserByName(string name, bool includeparking, bool includeFeedback);
    }
}
