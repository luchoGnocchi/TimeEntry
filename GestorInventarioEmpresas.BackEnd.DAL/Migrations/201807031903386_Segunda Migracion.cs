namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SegundaMigracion : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CompanyProyects", "Company_Id", "dbo.Companies");
            DropForeignKey("dbo.CompanyProyects", "Proyect_Id", "dbo.Proyects");
            DropIndex("dbo.CompanyProyects", new[] { "Company_Id" });
            DropIndex("dbo.CompanyProyects", new[] { "Proyect_Id" });
            CreateTable(
                "dbo.ProyectCompanies",
                c => new
                    {
                        Proyect_Id = c.Long(nullable: false),
                        Company_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Proyect_Id, t.Company_Id })
                .ForeignKey("dbo.Proyects", t => t.Proyect_Id, cascadeDelete: true)
                .ForeignKey("dbo.Companies", t => t.Company_Id, cascadeDelete: true)
                .Index(t => t.Proyect_Id)
                .Index(t => t.Company_Id);
            
            DropTable("dbo.CompanyProyects");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CompanyProyects",
                c => new
                    {
                        Company_Id = c.Long(nullable: false),
                        Proyect_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Company_Id, t.Proyect_Id });
            
            DropForeignKey("dbo.ProyectCompanies", "Company_Id", "dbo.Companies");
            DropForeignKey("dbo.ProyectCompanies", "Proyect_Id", "dbo.Proyects");
            DropIndex("dbo.ProyectCompanies", new[] { "Company_Id" });
            DropIndex("dbo.ProyectCompanies", new[] { "Proyect_Id" });
            DropTable("dbo.ProyectCompanies");
            CreateIndex("dbo.CompanyProyects", "Proyect_Id");
            CreateIndex("dbo.CompanyProyects", "Company_Id");
            AddForeignKey("dbo.CompanyProyects", "Proyect_Id", "dbo.Proyects", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CompanyProyects", "Company_Id", "dbo.Companies", "Id", cascadeDelete: true);
        }
    }
}
