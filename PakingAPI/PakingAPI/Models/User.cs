using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PakingAPI.Model
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        [Required, MinLength(2,ErrorMessage ="Name should be longer than 2 characters")]
        public string FirstName { get; set; }
        [Required, MinLength(2, ErrorMessage = "Name should be longer than 2 characters")]
        public string LastName { get; set; }    
        [Range(0, 150)]
        public int Age { get; set; }
        [RegularExpression(@"^[a-z0-9A-Z]{5, 30}.(com|se)$")]
        public string Email { get; set; }
        public ICollection<UserParking> UserParkings { get; set; }
    }
}
