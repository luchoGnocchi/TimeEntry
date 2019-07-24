using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorInventarioEmpresas.BackEnd.Domain.Entities
{
    [Serializable]
    public class BaseEntity
    {
        public long Id { get; set; }
        public bool IsDeletedByAdmin { get; set; }
    }
}
