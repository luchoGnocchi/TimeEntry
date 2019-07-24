namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _12ta : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Proyects", "Code", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Proyects", "Code");
        }
    }
}
