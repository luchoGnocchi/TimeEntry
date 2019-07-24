namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _19 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Companies", "Code", c => c.String(nullable: false, maxLength: 3));
            AlterColumn("dbo.Proyects", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Proyects", "Code", c => c.String(nullable: false, maxLength: 3));
            CreateIndex("dbo.Companies", "Code", unique: true);
            CreateIndex("dbo.Proyects", "Code", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Proyects", new[] { "Code" });
            DropIndex("dbo.Companies", new[] { "Code" });
            AlterColumn("dbo.Proyects", "Code", c => c.String());
            AlterColumn("dbo.Proyects", "Name", c => c.String());
            AlterColumn("dbo.Companies", "Code", c => c.String());
        }
    }
}
