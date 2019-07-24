namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _311 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfiles", "TypeEmployer", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfiles", "TypeEmployer");
        }
    }
}
