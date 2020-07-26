using PakingAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PakingAPI.Services
{
    public interface IParkingRepo
    {
        Task<Parking[]> GetAllParking(bool includeFeedback,bool includeUser);
        Task<Parking> GetParkingById(int id, bool includeFeedback,bool includeUser);
        Task<Parking> GetParkingByStreet(string streetName, bool includeFeedback, bool includeUser);
    }
}
