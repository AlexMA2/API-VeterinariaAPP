using System.ComponentModel.DataAnnotations;

namespace APIVeterinariaMimascot
{
    public class Specie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Nombre { get; set; }
    }
}
