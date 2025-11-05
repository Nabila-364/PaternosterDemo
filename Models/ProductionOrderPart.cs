using System.ComponentModel.DataAnnotations;

namespace PaternosterDemo.Models
{
    public class ProductionOrderPart
    {
        [Key]
        public int Id { get; set; } // EF Core heeft altijd 1 PK nodig

        [Required]
        public int OrderId { get; set; }
        public ProductionOrder ProductionOrder { get; set; }

        [Required]
        public int PartId { get; set; }
        public Part Part { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
