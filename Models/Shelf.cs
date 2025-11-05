using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaternosterDemo.Models
{
    public class Shelf
    {
        [Key]
        public int ShelfId { get; set; }

        [Required]
        [ForeignKey("Cabinet")]
        public int CabinetId { get; set; }

        public int MaxBins { get; set; }

        // Navigatieproperty naar parent Cabinet
        public Cabinet Cabinet { get; set; } = null!;

        // Navigatieproperty naar Bins
        public List<Bin> Bins { get; set; } = new List<Bin>();

        // Methode om een Bin toe te voegen
        public void AddBin(Bin bin)
        {
            if (Bins.Count >= MaxBins)
                throw new System.Exception("Maximaal aantal bakjes bereikt");

            bin.ShelfId = ShelfId;
            Bins.Add(bin);
        }
    }
}
