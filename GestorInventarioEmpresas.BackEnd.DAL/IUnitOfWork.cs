using GestorInventarioEmpresas.BackEnd.Domain;
using GestorInventarioEmpresas.BackEnd.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorInventarioEmpresas.BackEnd.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<UserProfile> UserRepository { get; }
        IRepository<Proyect> ProyectRepository { get; }
        
        void Save();
    }
}
