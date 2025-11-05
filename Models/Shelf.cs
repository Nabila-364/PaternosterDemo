using System.ComponentModel.DataAnnotations;

namespace PaternosterDemo.Models
{
    public class Shelf
    {
        public int ShelfId { get; set; }

        [Required(ErrorMessage = "Selecteer een kast")]
        public int CabinetId { get; set; }

        [Required(ErrorMessage = "Vul het schapnummer in")]
        public string ShelfNumber { get; set; } = null!;

        [Range(0, int.MaxValue, ErrorMessage = "Maximale gewicht moet positief zijn")]
        public int MaxWeight { get; set; }

        // Navigatie-eigenschappen
        public Cabinet? Cabinet { get; set; }

        public int MaxBins { get; set; }
    }
}
