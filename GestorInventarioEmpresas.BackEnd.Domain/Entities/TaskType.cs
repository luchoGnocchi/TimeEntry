using GestorInventarioEmpresas.BackEnd.Commons.Enums;
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
    public class TaskType : BaseEntity
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
        [Index(IsUnique = true)]
        public string Code { get; set; }
        public virtual List<InstanceDay> InstanceDay { get; set; } = new List<InstanceDay>();
        public bool Standard  {get;set;}
        public bool Mantenimiento { get;set;}
        public bool TimeOff { get;set;}
        public bool BackOffice { get;set; }
    }
}
