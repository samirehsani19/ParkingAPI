using PakingAPI.Hateoas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PakingAPI.DTO
{
    public class UserDTO:HateoasLinkBase
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public ICollection<ParkingDTO> Parkings { get; set; }
        public ICollection<FeedbackDTO> Feedbacks { get; set; }
    }
}
