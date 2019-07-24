namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _23 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Proyects", "TypeProyect", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Proyects", "TypeProyect");
        }
    }
}
