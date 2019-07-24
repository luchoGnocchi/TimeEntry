namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _21092018x : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfiles", "Location", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfiles", "Location");
        }
    }
}
