using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PaternosterDemo.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        [Required]
        public string Role { get; set; } = "Medewerker";

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
