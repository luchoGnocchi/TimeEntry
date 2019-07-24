namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _15ta : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InstanceDays", "TaskTypeId", c => c.Long());
            CreateIndex("dbo.InstanceDays", "TaskTypeId");
            AddForeignKey("dbo.InstanceDays", "TaskTypeId", "dbo.TaskTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InstanceDays", "TaskTypeId", "dbo.TaskTypes");
            DropIndex("dbo.InstanceDays", new[] { "TaskTypeId" });
            DropColumn("dbo.InstanceDays", "TaskTypeId");
        }
    }
}
