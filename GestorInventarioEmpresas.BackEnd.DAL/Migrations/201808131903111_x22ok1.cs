namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class x22ok1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Proyects", "TypeProyect_Id", "dbo.TypeProyects");
            DropIndex("dbo.Proyects", new[] { "TypeProyect_Id" });
            AddColumn("dbo.Proyects", "TypeProyect", c => c.Int(nullable: false));
            DropColumn("dbo.Proyects", "TypeProyect_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Proyects", "TypeProyect_Id", c => c.Long());
            DropColumn("dbo.Proyects", "TypeProyect");
            CreateIndex("dbo.Proyects", "TypeProyect_Id");
            AddForeignKey("dbo.Proyects", "TypeProyect_Id", "dbo.TypeProyects", "Id");
        }
    }
}
