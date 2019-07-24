using GestorInventarioEmpresas.BackEnd.Domain;
using GestorInventarioEmpresas.BackEnd.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorInventarioEmpresas.BackEnd.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private GestorInventarioEmpresasContext context;
        private GenericRepository<UserProfile> userRepository;
        private GenericRepository<Proyect> proyectRepository;
        public UnitOfWork(GestorInventarioEmpresasContext carManagmentContext)
        {
            this.context = carManagmentContext;
        }
        public UnitOfWork()
        {
            context =new  GestorInventarioEmpresasContext();
        }
        
        public IRepository<UserProfile> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new GenericRepository<UserProfile>(context);
                }
                return userRepository;
            }
        }
        public IRepository<Proyect> ProyectRepository
        {
            get
            {
                if (this.proyectRepository == null)
                {
                    this.proyectRepository = new GenericRepository<Proyect>(context);
                }
                return proyectRepository;
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}


