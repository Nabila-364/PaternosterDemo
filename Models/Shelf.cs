using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace PaternosterDemo.Models
{
    public class Shelf
    {
        [Key]
        public int ShelfId { get; set; }

        [Required]
        public int CabinetId { get; set; }
        public Cabinet Cabinet { get; set; }

        [Required]
        public int MaxBins { get; set; }

        public List<Bin> Bins { get; set; }
    }
}
