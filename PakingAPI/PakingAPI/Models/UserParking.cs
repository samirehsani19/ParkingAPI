using PakingAPI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PakingAPI
{
    public class UserParking
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int UserID { get; set; }
        public User User { get; set; }
        [Required]
        public int ParkingID { get; set; }
        public Parking Parking { get; set; }
        [Range(0, 10, ErrorMessage = "You can rate between 0 to 10")]
        public int? Rate { get; set; }
        public string Comment { get; set; }
    }
}
