using System;

namespace PaternosterDemo.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int PartId { get; set; }
        public Part? Part { get; set; }
        public int CabinetId { get; set; }
        public Cabinet? Cabinet { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public string? Action { get; set; } // "Haal" of "VoegToe"
        public int Quantity { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
