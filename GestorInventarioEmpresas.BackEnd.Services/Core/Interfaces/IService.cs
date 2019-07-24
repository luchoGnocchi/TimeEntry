using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorInventarioEmpresas.BackEnd.Services.Core.Interfaces
{
    public interface IService<TEntity> where TEntity : class, new()
    {
        TEntity Update( TEntity item);
        bool Exist(params object[] keyValues);
        void DeleteById( params object[] keyValues);
        void Add( TEntity item);
        TEntity GetbyId(params object[] keyValues);
        ICollection<TEntity> GetAll();
    }
}
