using System.Collections.Generic;

namespace PaternosterDemo.Models
{
    public class Part
    {
        public int PartId { get; set; }
        public string? ArticleNumber { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Inventory>? Inventories { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
    }
}
