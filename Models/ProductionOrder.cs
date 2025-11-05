using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PaternosterDemo.Models
{
    public class ProductionOrder
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Open"; // Open, InProgress, Completed, etc.

        // Navigatieproperty naar onderdelen
        public List<ProductionOrderPart> ProductionOrderParts { get; set; } = new List<ProductionOrderPart>();

        // Methode om onderdeel toe te voegen
        public void AddPart(Part part, int quantity)
        {
            if (part == null)
                throw new ArgumentNullException(nameof(part));

            ProductionOrderParts.Add(new ProductionOrderPart
            {
                PartId = part.PartId,
                Quantity = quantity,
                ProductionOrderOrderId = OrderId
            });
        }
    }
}
