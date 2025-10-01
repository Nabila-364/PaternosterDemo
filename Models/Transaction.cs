using System;
using System.ComponentModel.DataAnnotations;

namespace PaternosterDemo.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        [Required]
        public int InventoryId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int QuantityChanged { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;

        public Inventory? Inventory { get; set; }
        public User? User { get; set; }
    }
}
