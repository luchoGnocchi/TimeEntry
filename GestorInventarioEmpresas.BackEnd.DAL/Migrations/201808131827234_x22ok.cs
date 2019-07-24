namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class x22ok : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TypeProyects",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        IsDeletedByAdmin = c.Boolean(nullable: false),
                        TaskType_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TaskTypes", t => t.TaskType_Id)
                .Index(t => t.TaskType_Id);
            
            AddColumn("dbo.Proyects", "TypeProyect_Id", c => c.Long());
            CreateIndex("dbo.Proyects", "TypeProyect_Id");
            AddForeignKey("dbo.Proyects", "TypeProyect_Id", "dbo.TypeProyects", "Id");
            DropColumn("dbo.Proyects", "TypeProyect");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Proyects", "TypeProyect", c => c.Int(nullable: false));
            DropForeignKey("dbo.TypeProyects", "TaskType_Id", "dbo.TaskTypes");
            DropForeignKey("dbo.Proyects", "TypeProyect_Id", "dbo.TypeProyects");
            DropIndex("dbo.TypeProyects", new[] { "TaskType_Id" });
            DropIndex("dbo.Proyects", new[] { "TypeProyect_Id" });
            DropColumn("dbo.Proyects", "TypeProyect_Id");
            DropTable("dbo.TypeProyects");
        }
    }
}
