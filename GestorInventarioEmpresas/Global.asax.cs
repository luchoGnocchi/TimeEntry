using Autofac;
using Autofac.Integration.Mvc;
using GestorInventarioEmpresas.Models;
using GestorInventarioEmpresas.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Newtonsoft.Json;
using System.Web.Http;

namespace GestorInventarioEmpresas
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            var config = GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling
= Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            //registrando las dependencias
            var builder = new Autofac.ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
            builder.RegisterModule(new ServiceModule());
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new EntityFrameworkModule());
            //registrando los Filter
            builder.RegisterFilterProvider();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
