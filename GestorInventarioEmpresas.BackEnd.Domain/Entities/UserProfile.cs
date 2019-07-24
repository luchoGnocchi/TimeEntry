using GestorInventarioEmpresas.BackEnd.Commons.Enums;
using GestorInventarioEmpresas.BackEnd.Domain.Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GestorInventarioEmpresas.BackEnd.Domain
{
    public class UserProfile: BaseEntity
    {
        public string Name { get; set; }
        public string ProfileImage { get; set; }
        public DateTime Created { get; set; }
        public TypeEmployerEnum TypeEmployer { get; set; } 
        public LocationEnum Location { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }


}
