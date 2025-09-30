using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PaternosterDemo.Models
{
    public class Cabinet
    {
        public int CabinetId { get; set; }

        [Required]
        public string CabinetNumber { get; set; } = null!;

        [Required]
        public string Location { get; set; } = null!;

        public int Capacity { get; set; }

        public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
