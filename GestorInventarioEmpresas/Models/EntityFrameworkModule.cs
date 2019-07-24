using Autofac;
using GestorInventarioEmpresas.BackEnd.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;

namespace GestorInventarioEmpresas.Models
{
    public class EntityFrameworkModule : Autofac.Module
    {

         
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(GestorInventarioEmpresasContext)).As(typeof(DbContext)).InstancePerLifetimeScope();
        }
    }
 
}