
using GestorInventarioEmpresas.BackEnd.DAL;
using GestorInventarioEmpresas.BackEnd.Domain.Entities;
using GestorInventarioEmpresas.BackEnd.Services.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GestorInventarioEmpresas.BackEnd.Services.Core.Generics
{
  
public class GenericService<T> : IService<T> where T : BaseEntity, new()
	{
		protected IRepository<T> _genericRepository;

		public GenericService(IRepository<T> genericRepository)
		{
			_genericRepository = genericRepository;
		}

		public virtual void Add(T t)
		{
			_genericRepository.Insert(t);

		}

		public virtual void Delete( params object[] keyValues)
		{
			_genericRepository.Delete(keyValues);

		}

		public virtual void DeleteById( params object[] keyValues)
		{

            try
            {
		_genericRepository.Delete(keyValues);
            }
            catch (Exception)
            {

                throw;
            }
		

		}

		public virtual T GetbyId(params object[] keyValues)
		{
			return _genericRepository.GetByID(keyValues);
		}

		public virtual T Update( T item)
		{
            //NO FUNCIONA ID EN UPDATE HARDCODEADA
			return _genericRepository.Update(item, item.Id);
		}

		public virtual ICollection<T> GetAll()
		{
			return _genericRepository.GetAll().ToList();
		}
    
		public virtual bool Exist(params object[] keyValues)
		{
			return _genericRepository.GetByID(keyValues) != null;
		}
	}
}
