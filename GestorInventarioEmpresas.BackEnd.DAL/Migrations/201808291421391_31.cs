namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _31 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Proyects", "billable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Proyects", "billable");
        }
    }
}
