namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _30082018 : DbMigration
    {
        public override void Up()
        {
         //   AddColumn("dbo.UserProfiles", "TypeEmployer", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
          //  DropColumn("dbo.UserProfiles", "TypeEmployer");
        }
    }
}
