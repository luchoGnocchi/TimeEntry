using GestorInventarioEmpresas.BackEnd.DAL;
using GestorInventarioEmpresas.BackEnd.Services.Core.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorInventarioEmpresas.BackEnd.Services
{
  public class CompanyService : GenericService<Domain.Entities.Company>, ICompanyService
    {
        public CompanyService(IRepository<Domain.Entities.Company> genericRepository) : base(genericRepository)
        {
        }
    }
   
}
