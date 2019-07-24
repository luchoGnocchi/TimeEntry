namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6ta : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Companies", "WorkDay_Id", "dbo.WorkDays");
            DropIndex("dbo.Companies", new[] { "WorkDay_Id" });
            CreateTable(
                "dbo.WorkDayCompanies",
                c => new
                    {
                        WorkDay_Id = c.Long(nullable: false),
                        Company_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.WorkDay_Id, t.Company_Id })
                .ForeignKey("dbo.WorkDays", t => t.WorkDay_Id, cascadeDelete: true)
                .ForeignKey("dbo.Companies", t => t.Company_Id, cascadeDelete: true)
                .Index(t => t.WorkDay_Id)
                .Index(t => t.Company_Id);
            
            DropColumn("dbo.Companies", "WorkDay_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "WorkDay_Id", c => c.Long());
            DropForeignKey("dbo.WorkDayCompanies", "Company_Id", "dbo.Companies");
            DropForeignKey("dbo.WorkDayCompanies", "WorkDay_Id", "dbo.WorkDays");
            DropIndex("dbo.WorkDayCompanies", new[] { "Company_Id" });
            DropIndex("dbo.WorkDayCompanies", new[] { "WorkDay_Id" });
            DropTable("dbo.WorkDayCompanies");
            CreateIndex("dbo.Companies", "WorkDay_Id");
            AddForeignKey("dbo.Companies", "WorkDay_Id", "dbo.WorkDays", "Id");
        }
    }
}
