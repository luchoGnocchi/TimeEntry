using GestorInventarioEmpresas.BackEnd.DAL;
using GestorInventarioEmpresas.BackEnd.Services.Core.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorInventarioEmpresas.BackEnd.Services
{
  public class ProyectService : GenericService<Domain.Entities.Proyect>, IProyectService
    {
        public ProyectService(IRepository<Domain.Entities.Proyect> genericRepository) : base(genericRepository)
        {
        }
    }
   
}
