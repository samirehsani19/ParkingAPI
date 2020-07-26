using PakingAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PakingAPI.Model
{
    public class Parking
    {
        [Key]
        public int ParkingID { get; set; }
        [Required]
        [MinLength(2, ErrorMessage ="Name should be longer than 2 characters")]
        public string Country { get; set; }
        [Required]
        [MinLength(2, ErrorMessage ="Name should be longer than 2 characters"), MaxLength(25)]
        public string City { get; set; }
        [Required]
        public string StreetAdress { get; set; }    
        public string FreeParkingStart { get; set; }
        public string FreeParkingEnd { get; set; }
        public User User { get; set; }
        public int UserID { get; set; }
        public ICollection<Feedback> feedbacks { get; set; }
    }
}
