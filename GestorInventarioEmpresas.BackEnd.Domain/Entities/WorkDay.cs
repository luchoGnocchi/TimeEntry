using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorInventarioEmpresas.BackEnd.Domain.Entities
{
   public class WorkDay:BaseEntity
    {
       
        public long? UserProfileId { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public  DateTime Date { get; set; }
        public virtual List<InstanceDay> InstanceDay { get; set; } = new List<InstanceDay>();

    }
}
