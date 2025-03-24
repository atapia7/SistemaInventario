using System.ComponentModel.DataAnnotations;

namespace SistemaInventario.Modelos
{
    public class Marca
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage ="Nombre es Requerido")]
        [MaxLength(60,ErrorMessage ="longitud maxima 60 caracteres")]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "longitud maxima 60 caracteres")]
        public string  Descripcion  { get; set; }

        [Required(ErrorMessage ="estado required")]
        public bool Estado { get; set; }
    }
}
