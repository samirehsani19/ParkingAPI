using PakingAPI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PakingAPI.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackID { get; set; }
        public int ParkingID { get; set; }
        public Parking Parking { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        [Range(0, 10, ErrorMessage = "You can rate between 0 to 10")]
        public int? Rate { get; set; }
        [MinLength(3, ErrorMessage ="Comment should be longer than three characters")]
        public string Comment { get; set; }
    }
}
