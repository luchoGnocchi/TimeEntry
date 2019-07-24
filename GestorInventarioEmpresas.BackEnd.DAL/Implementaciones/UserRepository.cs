using GestorInventarioEmpresas.BackEnd.DAL.Interfaces;
using GestorInventarioEmpresas.BackEnd.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorInventarioEmpresas.BackEnd.DAL.Implementaciones
{
    public class UserRepository : GenericRepository<UserProfile>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }
    }
}
