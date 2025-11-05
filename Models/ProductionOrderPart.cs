using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaternosterDemo.Models
{
    public class ProductionOrderPart
    {
        [Key]
        [Column("Id")]  // bestaande kolom in DB
        public int ProductionOrderPartId { get; set; }

        [Column("ProductionOrderOrderId")]  // bestaande kolom in DB
        [Display(Name = "Order")]
        public int ProductionOrderId { get; set; }

        [Display(Name = "Part")]
        public int PartId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        // Navigatie properties
        public ProductionOrder? ProductionOrder { get; set; }
        public Part? Part { get; set; }
    }
}
