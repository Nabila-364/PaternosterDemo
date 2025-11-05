using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PaternosterDemo.Models
{
    public class Cabinet
    {
        public int CabinetId { get; set; }

        [Required(ErrorMessage = "Vul het kastnummer in")]
        public string CabinetNumber { get; set; } = null!;

        [Required(ErrorMessage = "Vul de locatie in")]
        public string Location { get; set; } = null!;

        public string? Name { get; set; }  // Optioneel

        [Range(0, int.MaxValue, ErrorMessage = "Capaciteit moet positief zijn")]
        public int Capacity { get; set; }

        public ICollection<Shelf> Shelves { get; set; } = new List<Shelf>();
        public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
