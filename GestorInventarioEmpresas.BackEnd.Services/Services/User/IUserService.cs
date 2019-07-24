using GestorInventarioEmpresas.BackEnd.Domain.Entities;
using GestorInventarioEmpresas.BackEnd.Services.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorInventarioEmpresas.BackEnd.Services
{
    public interface IUserService : IService<GestorInventarioEmpresas.BackEnd.Domain.UserProfile>
    {
    }
    
}
