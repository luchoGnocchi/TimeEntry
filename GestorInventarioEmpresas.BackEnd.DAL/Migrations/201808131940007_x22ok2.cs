namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class x22ok2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TypeProyects", "TaskType_Id", "dbo.TaskTypes");
            DropIndex("dbo.TypeProyects", new[] { "TaskType_Id" });
            AddColumn("dbo.TaskTypes", "Standard", c => c.Boolean(nullable: false));
            AddColumn("dbo.TaskTypes", "Mantenimiento", c => c.Boolean(nullable: false));
            AddColumn("dbo.TaskTypes", "TimeOff", c => c.Boolean(nullable: false));
            AddColumn("dbo.TaskTypes", "BackOffice", c => c.Boolean(nullable: false));
            DropColumn("dbo.TypeProyects", "TaskType_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TypeProyects", "TaskType_Id", c => c.Long());
            DropColumn("dbo.TaskTypes", "BackOffice");
            DropColumn("dbo.TaskTypes", "TimeOff");
            DropColumn("dbo.TaskTypes", "Mantenimiento");
            DropColumn("dbo.TaskTypes", "Standard");
            CreateIndex("dbo.TypeProyects", "TaskType_Id");
            AddForeignKey("dbo.TypeProyects", "TaskType_Id", "dbo.TaskTypes", "Id");
        }
    }
}
