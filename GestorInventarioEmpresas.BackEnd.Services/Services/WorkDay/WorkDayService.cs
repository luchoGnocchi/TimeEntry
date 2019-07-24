using GestorInventarioEmpresas.BackEnd.DAL;
using GestorInventarioEmpresas.BackEnd.Services.Core.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorInventarioEmpresas.BackEnd.Services
{
  public class WorkDayService : GenericService<Domain.Entities.WorkDay>, IWorkDayService
    {
        public WorkDayService(IRepository<Domain.Entities.WorkDay> genericRepository) : base(genericRepository)
        {
        }
    }
   
}
