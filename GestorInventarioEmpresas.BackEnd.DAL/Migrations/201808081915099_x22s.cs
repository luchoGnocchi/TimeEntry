namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class x22s : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Proyects", "isActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Proyects", "isActive");
        }
    }
}
