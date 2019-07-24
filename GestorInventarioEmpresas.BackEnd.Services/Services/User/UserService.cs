using GestorInventarioEmpresas.BackEnd.DAL;

using GestorInventarioEmpresas.BackEnd.Services.Core.Generics;


namespace GestorInventarioEmpresas.BackEnd.Services
{
    public class UserService : GenericService<Domain.UserProfile>, IUserService
    {
        public UserService(IRepository<GestorInventarioEmpresas.BackEnd.Domain.UserProfile> genericRepository) : base(genericRepository)
        {
        }
    }

}
