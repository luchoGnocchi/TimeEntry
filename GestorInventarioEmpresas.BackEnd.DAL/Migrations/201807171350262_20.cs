namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Companies", new[] { "Code" });
            AlterColumn("dbo.Companies", "Code", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Companies", "Code", c => c.String(nullable: false, maxLength: 3));
            CreateIndex("dbo.Companies", "Code", unique: true);
        }
    }
}
