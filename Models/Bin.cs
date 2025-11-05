using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaternosterDemo.Models
{
    public class Bin
    {
        [Key] // <-- dit is belangrijk
        public int BinId { get; set; }

        [Required]
        public int ShelfId { get; set; }
        public Shelf Shelf { get; set; }  // navigatie property

        [Required]
        public int PartId { get; set; }
        public Part Part { get; set; } // navigatie property

        [Required]
        public int Quantity { get; set; }
    }
}
