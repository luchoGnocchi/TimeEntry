namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _9ta : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WorkDayCompanies", "WorkDay_Id", "dbo.WorkDays");
            DropForeignKey("dbo.WorkDayCompanies", "Company_Id", "dbo.Companies");
            DropForeignKey("dbo.WorkDays", "ProyectId", "dbo.Proyects");
            DropIndex("dbo.WorkDays", new[] { "ProyectId" });
            DropIndex("dbo.WorkDayCompanies", new[] { "WorkDay_Id" });
            DropIndex("dbo.WorkDayCompanies", new[] { "Company_Id" });
            CreateTable(
                "dbo.InstanceDays",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Hours = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProyectId = c.Long(nullable: false),
                        WorkDayId = c.Long(nullable: false),
                        IsDeletedByAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Proyects", t => t.ProyectId, cascadeDelete: true)
                .ForeignKey("dbo.WorkDays", t => t.WorkDayId, cascadeDelete: true)
                .Index(t => t.ProyectId)
                .Index(t => t.WorkDayId);
            
            AddColumn("dbo.Companies", "InstanceDay_Id", c => c.Long());
            AddColumn("dbo.WorkDays", "Company_Id", c => c.Long());
            CreateIndex("dbo.Companies", "InstanceDay_Id");
            CreateIndex("dbo.WorkDays", "Company_Id");
            AddForeignKey("dbo.Companies", "InstanceDay_Id", "dbo.InstanceDays", "Id");
            AddForeignKey("dbo.WorkDays", "Company_Id", "dbo.Companies", "Id");
            DropColumn("dbo.WorkDays", "Hours");
            DropColumn("dbo.WorkDays", "ProyectId");
            DropTable("dbo.WorkDayCompanies");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.WorkDayCompanies",
                c => new
                    {
                        WorkDay_Id = c.Long(nullable: false),
                        Company_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.WorkDay_Id, t.Company_Id });
            
            AddColumn("dbo.WorkDays", "ProyectId", c => c.Long());
            AddColumn("dbo.WorkDays", "Hours", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropForeignKey("dbo.WorkDays", "Company_Id", "dbo.Companies");
            DropForeignKey("dbo.InstanceDays", "WorkDayId", "dbo.WorkDays");
            DropForeignKey("dbo.InstanceDays", "ProyectId", "dbo.Proyects");
            DropForeignKey("dbo.Companies", "InstanceDay_Id", "dbo.InstanceDays");
            DropIndex("dbo.InstanceDays", new[] { "WorkDayId" });
            DropIndex("dbo.InstanceDays", new[] { "ProyectId" });
            DropIndex("dbo.WorkDays", new[] { "Company_Id" });
            DropIndex("dbo.Companies", new[] { "InstanceDay_Id" });
            DropColumn("dbo.WorkDays", "Company_Id");
            DropColumn("dbo.Companies", "InstanceDay_Id");
            DropTable("dbo.InstanceDays");
            CreateIndex("dbo.WorkDayCompanies", "Company_Id");
            CreateIndex("dbo.WorkDayCompanies", "WorkDay_Id");
            CreateIndex("dbo.WorkDays", "ProyectId");
            AddForeignKey("dbo.WorkDays", "ProyectId", "dbo.Proyects", "Id");
            AddForeignKey("dbo.WorkDayCompanies", "Company_Id", "dbo.Companies", "Id", cascadeDelete: true);
            AddForeignKey("dbo.WorkDayCompanies", "WorkDay_Id", "dbo.WorkDays", "Id", cascadeDelete: true);
        }
    }
}
