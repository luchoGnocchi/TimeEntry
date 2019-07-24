namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _30082017 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UserProfiles", "TypeEmployer");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserProfiles", "TypeEmployer", c => c.Int(nullable: false));
        }
    }
}
