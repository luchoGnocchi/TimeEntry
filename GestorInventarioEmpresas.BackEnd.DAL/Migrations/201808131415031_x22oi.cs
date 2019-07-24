namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class x22oi : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TaskTypes", "WorkDay_Id", "dbo.WorkDays");
            DropIndex("dbo.TaskTypes", new[] { "WorkDay_Id" });
            DropColumn("dbo.TaskTypes", "WorkDay_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TaskTypes", "WorkDay_Id", c => c.Long());
            CreateIndex("dbo.TaskTypes", "WorkDay_Id");
            AddForeignKey("dbo.TaskTypes", "WorkDay_Id", "dbo.WorkDays", "Id");
        }
    }
}
