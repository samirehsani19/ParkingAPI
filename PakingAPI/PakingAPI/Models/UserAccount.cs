using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PakingAPI.Models
{
    public class UserAccount
    {
        [Key]
        public int ID { get; set; }
        [MinLength(2, ErrorMessage ="Username should be longer than two characters")]
        public string Username { get; set; }
        [RegularExpression(@"^[a-z0-9A-Z]{3,30}")]
        [MinLength(3)]
        public string Password { get; set; }
    }
}
