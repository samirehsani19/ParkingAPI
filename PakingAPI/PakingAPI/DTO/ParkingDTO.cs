using PakingAPI.Hateoas;
using PakingAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PakingAPI.DTO
{
    public class ParkingDTO:HateoasLinkBase
    {
        public int ParkingID { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string StreetAdress { get; set; }
        public string FreeParkingStart { get; set; }
        public string FreeParkingEnd { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public ICollection<FeedbackDTO> feedbacks { get; set; }
    }
}
