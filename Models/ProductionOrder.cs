using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PaternosterDemo.Models
{
    public class ProductionOrder
    {
        [Key] // Primaire sleutel voor EF Core
        public int OrderId { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Open"; // bv. Open, InProgress, Completed

        // Navigatieproperty naar M:N relatie met Part via ProductionOrderPart
        public List<ProductionOrderPart> ProductionOrderParts { get; set; } = new List<ProductionOrderPart>();

        // Methode om onderdeel toe te voegen aan deze order
        public void AddPart(Part part, int quantity)
        {
            if (part == null) throw new ArgumentNullException(nameof(part));
            if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero");

            var existing = ProductionOrderParts.Find(p => p.PartId == part.PartId);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                ProductionOrderParts.Add(new ProductionOrderPart
                {
                    PartId = part.PartId,
                    Quantity = quantity
                });
            }
        }
    }
}
