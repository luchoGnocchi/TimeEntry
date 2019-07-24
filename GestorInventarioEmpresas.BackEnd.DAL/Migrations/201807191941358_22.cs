namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _22 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Companies", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Proyects", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Proyects", "Code", c => c.String(nullable: false, maxLength: 3));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Proyects", "Code", c => c.String());
            AlterColumn("dbo.Proyects", "Name", c => c.String());
            AlterColumn("dbo.Companies", "Name", c => c.String());
        }
    }
}
