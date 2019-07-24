namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _16ta : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InstanceDays", "TaskTypeId", "dbo.TaskTypes");
            DropIndex("dbo.InstanceDays", new[] { "TaskTypeId" });
            AlterColumn("dbo.InstanceDays", "TaskTypeId", c => c.Long(nullable: false));
            CreateIndex("dbo.InstanceDays", "TaskTypeId");
            AddForeignKey("dbo.InstanceDays", "TaskTypeId", "dbo.TaskTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InstanceDays", "TaskTypeId", "dbo.TaskTypes");
            DropIndex("dbo.InstanceDays", new[] { "TaskTypeId" });
            AlterColumn("dbo.InstanceDays", "TaskTypeId", c => c.Long());
            CreateIndex("dbo.InstanceDays", "TaskTypeId");
            AddForeignKey("dbo.InstanceDays", "TaskTypeId", "dbo.TaskTypes", "Id");
        }
    }
}
