namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _21 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Proyects", new[] { "Code" });
            AlterColumn("dbo.Companies", "Code", c => c.String(nullable: false, maxLength: 3));
            AlterColumn("dbo.Proyects", "Name", c => c.String());
            AlterColumn("dbo.Proyects", "Code", c => c.String());
            CreateIndex("dbo.Companies", "Code", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Companies", new[] { "Code" });
            AlterColumn("dbo.Proyects", "Code", c => c.String(nullable: false, maxLength: 3));
            AlterColumn("dbo.Proyects", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Companies", "Code", c => c.String());
            CreateIndex("dbo.Proyects", "Code", unique: true);
        }
    }
}
