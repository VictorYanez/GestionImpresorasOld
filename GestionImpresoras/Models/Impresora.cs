using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionImpresoras.Models
{
    [Table("Impresora")]
     public class Impresora
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "No. Activo Fijo")] 
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(22)]
        public string  CodigoActivoFijo { get; set; }

        [StringLength(50)]
        public string Marca { get; set; }

        [StringLength(50)]
        public string Modelo { get; set; }

        public int EstadoId { get; set; }  
        [Display(Name = "Estado")]
        public virtual Estado Estado { get; set; } = null!; //Perdonar el nulo? y Ademas propiedad de navegacion 

        [Display(Name = "Color")]
        public int EsdeColor { get; set; }

        [StringLength(15)]
        [Display(Name = "IP")]
        public string DireccionIP { get; set; }
        [StringLength(150)]
        public string Caracteristicas { get; set; }
    }
}
