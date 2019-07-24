using GestorInventarioEmpresas.BackEnd.Domain;
using GestorInventarioEmpresas.BackEnd.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorInventarioEmpresas.BackEnd.DAL
{
    public class GestorInventarioEmpresasContext : IdentityDbContext<ApplicationUser>
    {
        private const string ConnectionStringName = "Name=GestorInventarioEmpresasContext";

        public virtual DbSet<UserProfile> Users { get; set; }
        public virtual DbSet<Proyect> Proyects { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<TaskType> TaskTypes { get; set; }
        public virtual DbSet<WorkDay> WorkDays { get; set; }
        public virtual DbSet<InstanceDay> InstanceDays { get; set; }
        public virtual DbSet<TypeProyect> TypeProyects { get; set; }
        
        public static GestorInventarioEmpresasContext Create()
        {
            return new GestorInventarioEmpresasContext();
        }
        public GestorInventarioEmpresasContext() : base(ConnectionStringName, throwIfV1Schema: false)
        {
        }

        public void EmptyContext()
        {
            Users.RemoveRange(Users);
            Proyects.RemoveRange(Proyects);
            Companies.RemoveRange(Companies);
            TaskTypes.RemoveRange(TaskTypes);
            WorkDays.RemoveRange(WorkDays);
            InstanceDays.RemoveRange(InstanceDays);
            TypeProyects.RemoveRange(TypeProyects);
            
        }

       
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure ApplicationUser & StudentAddress entity
            modelBuilder.Entity<ApplicationUser>()
                        .HasOptional(u => u.UserProfile) // Mark UserProfile property optional in ApplicationUser entity
                        .WithRequired(c => c.ApplicationUser); // mark ApplicationUser property as required in UserProfile entity. Cannot save UserProfile without ApplicationUser
                                                               //.Map(c => c.MapKey("ApplicationUserId")); 
        }
    }
}
