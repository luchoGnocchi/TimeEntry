namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _11ta : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "Code", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "Code");
        }
    }
}
