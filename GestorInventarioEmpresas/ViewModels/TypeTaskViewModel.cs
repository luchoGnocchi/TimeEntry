using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestorInventarioEmpresas.ViewModels
{
    public class TypeTaskViewModel
    {
        [Required(ErrorMessage = "An Album Title is required")]
        [DisplayName("Nombre")]
        public string Name { get; set; }
        [DisplayName("Codigo")]
        [StringLength(160)]
        public string Code { get; set; }
    }
}