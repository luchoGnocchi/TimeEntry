namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Proyects",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        IsDeletedByAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        IsDeletedByAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CompanyProyects",
                c => new
                    {
                        Company_Id = c.Long(nullable: false),
                        Proyect_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Company_Id, t.Proyect_Id })
                .ForeignKey("dbo.Companies", t => t.Company_Id, cascadeDelete: true)
                .ForeignKey("dbo.Proyects", t => t.Proyect_Id, cascadeDelete: true)
                .Index(t => t.Company_Id)
                .Index(t => t.Proyect_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyProyects", "Proyect_Id", "dbo.Proyects");
            DropForeignKey("dbo.CompanyProyects", "Company_Id", "dbo.Companies");
            DropIndex("dbo.CompanyProyects", new[] { "Proyect_Id" });
            DropIndex("dbo.CompanyProyects", new[] { "Company_Id" });
            DropTable("dbo.CompanyProyects");
            DropTable("dbo.Companies");
            DropTable("dbo.Proyects");
        }
    }
}
