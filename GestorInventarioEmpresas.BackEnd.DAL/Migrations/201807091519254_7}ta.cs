namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _7ta : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TaskTypes", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.TaskTypes", "Code", c => c.String(maxLength: 3));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TaskTypes", "Code", c => c.String());
            AlterColumn("dbo.TaskTypes", "Name", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
