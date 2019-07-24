namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GestorInventarioEmpresas.BackEnd.DAL.GestorInventarioEmpresasContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "GestorInventarioEmpresas.BackEnd.DAL.GestorInventarioEmpresasContext";
        }

        protected override void Seed(GestorInventarioEmpresas.BackEnd.DAL.GestorInventarioEmpresasContext context)
        {
            //context.TaskTypes.AddOrUpdate(new Domain.Entities.TaskType() { Name = "Vacaciones", Code = "Vac", Id = 1 });
            //context.TaskTypes.AddOrUpdate(new Domain.Entities.TaskType() { Name = "Feriados", Code = "Fer", Id =2 });
            //context.TaskTypes.AddOrUpdate(new Domain.Entities.TaskType() { Name = "Dia Estudio", Code = "DES", Id =3 });
            //context.TaskTypes.AddOrUpdate(new Domain.Entities.TaskType() { Name = "Dia por Enfermedad", Code = "DEN", Id = 4});
            //context.TaskTypes.AddOrUpdate(new Domain.Entities.TaskType() { Name = "Licencia especial", Code = "LES", Id = 5 });
       }
    }
}
