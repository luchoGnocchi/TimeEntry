using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using GestorInventarioEmpresas.BackEnd.Domain;
using Model.Auth;

namespace GestorInventarioEmpresas.BackEnd.Services
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, string> roleStore)
        : base(roleStore) { }

        public static ApplicationRoleManager Create(
            IdentityFactoryOptions<ApplicationRoleManager> options,
            IOwinContext context)
        {
            var manager = new ApplicationRoleManager(
                new RoleStore<ApplicationRole>(context.Get<IdentityDbContext>()));
            return manager;
        }
    }
}
