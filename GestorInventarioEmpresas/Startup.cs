using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GestorInventarioEmpresas.Startup))]
namespace GestorInventarioEmpresas
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
