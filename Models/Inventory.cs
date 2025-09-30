using System.ComponentModel.DataAnnotations;

namespace PaternosterDemo.Models
{
    public class Inventory
    {
        public int InventoryId { get; set; }

        [Required]
        public int PartId { get; set; }

        [Required]
        public int CabinetId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        // Navigatieproperties, maak nullable zodat EF Core ze niet als required valideert
        public Part? Part { get; set; }
        public Cabinet? Cabinet { get; set; }
    }
}
