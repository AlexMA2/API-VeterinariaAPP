using System.ComponentModel.DataAnnotations;

namespace APIVeterinariaMimascot
{
    public class Pet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Nombre { get; set; }
        
        public double Edad { get; set; }

        public string? Genero { get; set; }

        [Required]
        public string? Especie { get; set; }

        [Required]
        public string? Problema { get; set; }

        [Required]
        public DateTime FechaReservada { get; set; }

    }
}
