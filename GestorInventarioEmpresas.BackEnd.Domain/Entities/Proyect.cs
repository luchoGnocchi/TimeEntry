using GestorInventarioEmpresas.BackEnd.Commons.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorInventarioEmpresas.BackEnd.Domain.Entities
{
    public class Proyect : BaseEntity
    {

        [DisplayName("Descripción")]
        [Required(ErrorMessage = "La Descripción es requerida")]
        [MinLength(3, ErrorMessage = "La Descripción debe tener un minimo de 3 caracteres alfanuméricos")]
        [MaxLength(50, ErrorMessage = "La Descripción debe tener un máximo de 50 caracteres alfanuméricos")] 
        public string Name { get; set; }
        [DisplayName("Código")]
        [Required(ErrorMessage = "Debe ingresar un código")]
        [MinLength(3, ErrorMessage = "El código debe tener 3 caracteres alfanuméricos")]
        [MaxLength(3, ErrorMessage = "El código debe tener 3 caracteres alfanuméricos")]
        public string Code { get; set; }
        public virtual List<Company> Companies { get; set; } = new List<Company>();
        public TypeProyectEnum TypeProyect { get; set; }
        public bool isActive { get; set; }
        public bool billable { get; set; }

    }
}
