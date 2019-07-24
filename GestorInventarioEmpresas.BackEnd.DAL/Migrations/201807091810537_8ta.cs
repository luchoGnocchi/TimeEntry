namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8ta : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TaskTypes", "Code", c => c.String(nullable: false, maxLength: 3));
            CreateIndex("dbo.TaskTypes", "Code", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.TaskTypes", new[] { "Code" });
            AlterColumn("dbo.TaskTypes", "Code", c => c.String(maxLength: 3));
        }
    }
}
