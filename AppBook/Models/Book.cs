using System.ComponentModel.DataAnnotations;

namespace NumePrenume.Models
{
    public class Book
    {
        public int ID { get; set; }

        [Required]
        public string? Denumire { get; set; }

        [Required]
        public string? Autor { get; set; }

        [Required]
        public string? Editura { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Prețul trebuie să fie un număr pozitiv.")]
        public double Pret { get; set; }

        public string? ImagePath { get; set; }  // Calea pentru imagine
    }
}
