using GestorInventarioEmpresas.BackEnd.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorInventarioEmpresas.BackEnd.DAL.Interfaces
{
    public interface IUserRepository : IRepository<UserProfile>
    {
    }
}
