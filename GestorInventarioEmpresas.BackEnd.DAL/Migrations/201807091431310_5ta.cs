namespace GestorInventarioEmpresas.BackEnd.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5ta : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkDays",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserProfileId = c.Long(),
                        Date = c.DateTime(nullable: false),
                        Hours = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProyectId = c.Long(),
                        IsDeletedByAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Proyects", t => t.ProyectId)
                .ForeignKey("dbo.UserProfiles", t => t.UserProfileId)
                .Index(t => t.UserProfileId)
                .Index(t => t.ProyectId);
            
            AddColumn("dbo.Companies", "WorkDay_Id", c => c.Long());
            AlterColumn("dbo.TaskTypes", "Name", c => c.String(nullable: false, maxLength: 10));
            CreateIndex("dbo.Companies", "WorkDay_Id");
            AddForeignKey("dbo.Companies", "WorkDay_Id", "dbo.WorkDays", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkDays", "UserProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.WorkDays", "ProyectId", "dbo.Proyects");
            DropForeignKey("dbo.Companies", "WorkDay_Id", "dbo.WorkDays");
            DropIndex("dbo.WorkDays", new[] { "ProyectId" });
            DropIndex("dbo.WorkDays", new[] { "UserProfileId" });
            DropIndex("dbo.Companies", new[] { "WorkDay_Id" });
            AlterColumn("dbo.TaskTypes", "Name", c => c.String());
            DropColumn("dbo.Companies", "WorkDay_Id");
            DropTable("dbo.WorkDays");
        }
    }
}
