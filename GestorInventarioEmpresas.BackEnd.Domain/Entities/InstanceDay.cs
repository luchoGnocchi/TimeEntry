using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorInventarioEmpresas.BackEnd.Domain.Entities
{
 public   class InstanceDay:BaseEntity
    {
       
        public decimal Hours { get; set; }
        public long ProyectId { get; set; }
        public virtual Proyect Proyect { get; set; }
        public long  WorkDayId { get; set; }
        public virtual WorkDay  WorkDay { get; set; }
        public virtual List<Company> Companies { get; set; }
        public long TaskTypeId { get; set; }
        public virtual TaskType TaskType { get; set; }


    }
}
