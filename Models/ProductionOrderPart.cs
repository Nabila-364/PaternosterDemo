using System.ComponentModel.DataAnnotations;

namespace PaternosterDemo.Models
{
    public class ProductionOrderPart
    {
        public int Id { get; set; }

        [Display(Name = "Order")]
        public int ProductionOrderOrderId { get; set; }

        [Display(Name = "Part")]
        public int PartId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        // Maak navigatie properties nullable
        public ProductionOrder? ProductionOrder { get; set; }
        public Part? Part { get; set; }
    }
}
