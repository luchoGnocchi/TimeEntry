namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class x22 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Proyects", "isActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Proyects", "isActive", c => c.Boolean(nullable: false));
        }
    }
}
