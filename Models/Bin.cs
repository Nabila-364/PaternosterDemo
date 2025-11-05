using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaternosterDemo.Models
{
    public class Bin
    {
        [Key]
        public int BinId { get; set; }

        [Required]
        [ForeignKey("Shelf")]
        public int ShelfId { get; set; }

        [Required]
        [ForeignKey("Part")]
        public int PartId { get; set; }

        public int Quantity { get; set; }

        // Navigatieproperties
        public Shelf Shelf { get; set; }
        public Part Part { get; set; }

        public void UpdateContents(int amount)
        {
            if (Quantity + amount < 0)
                throw new System.Exception("Niet genoeg voorraad beschikbaar");
            Quantity += amount;
        }
    }
}
