namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _10ta : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Companies", "InstanceDay_Id", "dbo.InstanceDays");
            DropForeignKey("dbo.WorkDays", "Company_Id", "dbo.Companies");
            DropIndex("dbo.Companies", new[] { "InstanceDay_Id" });
            DropIndex("dbo.WorkDays", new[] { "Company_Id" });
            CreateTable(
                "dbo.InstanceDayCompanies",
                c => new
                    {
                        InstanceDay_Id = c.Long(nullable: false),
                        Company_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.InstanceDay_Id, t.Company_Id })
                .ForeignKey("dbo.InstanceDays", t => t.InstanceDay_Id, cascadeDelete: true)
                .ForeignKey("dbo.Companies", t => t.Company_Id, cascadeDelete: true)
                .Index(t => t.InstanceDay_Id)
                .Index(t => t.Company_Id);
            
            DropColumn("dbo.Companies", "InstanceDay_Id");
            DropColumn("dbo.WorkDays", "Company_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WorkDays", "Company_Id", c => c.Long());
            AddColumn("dbo.Companies", "InstanceDay_Id", c => c.Long());
            DropForeignKey("dbo.InstanceDayCompanies", "Company_Id", "dbo.Companies");
            DropForeignKey("dbo.InstanceDayCompanies", "InstanceDay_Id", "dbo.InstanceDays");
            DropIndex("dbo.InstanceDayCompanies", new[] { "Company_Id" });
            DropIndex("dbo.InstanceDayCompanies", new[] { "InstanceDay_Id" });
            DropTable("dbo.InstanceDayCompanies");
            CreateIndex("dbo.WorkDays", "Company_Id");
            CreateIndex("dbo.Companies", "InstanceDay_Id");
            AddForeignKey("dbo.WorkDays", "Company_Id", "dbo.Companies", "Id");
            AddForeignKey("dbo.Companies", "InstanceDay_Id", "dbo.InstanceDays", "Id");
        }
    }
}
