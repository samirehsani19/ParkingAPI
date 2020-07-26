using PakingAPI.Hateoas;
using PakingAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PakingAPI.DTO
{
    public class FeedbackDTO:HateoasLinkBase
    {
        public int FeedbackID { get; set; }
        public int? Rate { get; set; }
        public string Comment { get; set; }
        public int UserID { get; set; }
        public int ParkingID { get; set; }
        public User User { get; set; }
        public Parking Parking { get; set; }
    }
}
