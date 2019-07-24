namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _24092018x : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "Location", c => c.Int(nullable: false));
            DropColumn("dbo.Proyects", "Location");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Proyects", "Location", c => c.Int(nullable: false));
            DropColumn("dbo.Companies", "Location");
        }
    }
}
