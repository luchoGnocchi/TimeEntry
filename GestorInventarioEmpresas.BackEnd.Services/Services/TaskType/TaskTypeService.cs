using GestorInventarioEmpresas.BackEnd.DAL;
using GestorInventarioEmpresas.BackEnd.Services.Core.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorInventarioEmpresas.BackEnd.Services
{
  public class TaskTypeService : GenericService<Domain.Entities.TaskType>, ITaskTypeService
    {
        public TaskTypeService(IRepository<Domain.Entities.TaskType> genericRepository) : base(genericRepository)
        {
        }
    }
   
}
