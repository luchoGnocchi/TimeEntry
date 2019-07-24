using GestorInventarioEmpresas.BackEnd.DAL;
using GestorInventarioEmpresas.BackEnd.Services.Core.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorInventarioEmpresas.BackEnd.Services
{
  public class InstanceDayService : GenericService<Domain.Entities.InstanceDay>, IInstanceDayService
    {
        public InstanceDayService(IRepository<Domain.Entities.InstanceDay> genericRepository) : base(genericRepository)
        {
        }
    }
   
}
