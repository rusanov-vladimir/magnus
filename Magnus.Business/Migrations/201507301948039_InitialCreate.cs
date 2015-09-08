namespace Magnus.Business.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DynamicFieldTemplates",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(nullable: false, maxLength: 10),
                        Type = c.Int(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
                        Label = c.String(),
                        Weight = c.Int(nullable: false),
                        Length = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true, name: "IX_UQ_Code");
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 128),
                        WorkingDirectory = c.String(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DynamicFields",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Configuration_Id = c.Guid(nullable: false),
                        Project_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DynamicFieldTemplates", t => t.Configuration_Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .Index(t => t.Configuration_Id)
                .Index(t => t.Project_Id);
            
            CreateTable(
                "dbo.DynamicFieldValues",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        BoolValue = c.Boolean(),
                        DateTimeValue = c.DateTime(),
                        DoubleValue = c.Double(),
                        IntegerValue = c.Int(),
                        StringValue = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DynamicFields", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 128),
                        Project_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .Index(t => t.Project_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FirstName = c.String(maxLength: 64),
                        LastName = c.String(maxLength: 64),
                        Username = c.String(nullable: false, maxLength: 64),
                        Password = c.String(),
                        IsAdvancedUser = c.Boolean(nullable: false),
                        Team_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.Team_Id)
                .Index(t => t.Team_Id);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CurrentDocumentId = c.Guid(),
                        Name = c.String(),
                        CurrentUser_Id = c.Guid(),
                        Project_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CurrentUser_Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .Index(t => t.CurrentUser_Id)
                .Index(t => t.Project_Id);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 256),
                        Task_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tasks", t => t.Task_Id)
                .Index(t => t.Task_Id);
            
            CreateTable(
                "dbo.ProjectDynamicFieldTemplates",
                c => new
                    {
                        Project_Id = c.Guid(nullable: false),
                        DynamicFieldTemplate_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.DynamicFieldTemplate_Id })
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .ForeignKey("dbo.DynamicFieldTemplates", t => t.DynamicFieldTemplate_Id, cascadeDelete: true)
                .Index(t => t.Project_Id)
                .Index(t => t.DynamicFieldTemplate_Id);
            
            CreateTable(
                "dbo.DocumentDynamicFields",
                c => new
                    {
                        Document_Id = c.Guid(nullable: false),
                        DynamicField_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Document_Id, t.DynamicField_Id })
                .ForeignKey("dbo.Documents", t => t.Document_Id, cascadeDelete: true)
                .ForeignKey("dbo.DynamicFields", t => t.DynamicField_Id, cascadeDelete: true)
                .Index(t => t.Document_Id)
                .Index(t => t.DynamicField_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Documents", "Task_Id", "dbo.Tasks");
            DropForeignKey("dbo.DocumentDynamicFields", "DynamicField_Id", "dbo.DynamicFields");
            DropForeignKey("dbo.DocumentDynamicFields", "Document_Id", "dbo.Documents");
            DropForeignKey("dbo.Tasks", "CurrentUser_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.Teams", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.ProjectDynamicFieldTemplates", "DynamicFieldTemplate_Id", "dbo.DynamicFieldTemplates");
            DropForeignKey("dbo.ProjectDynamicFieldTemplates", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.DynamicFields", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.DynamicFieldValues", "Id", "dbo.DynamicFields");
            DropForeignKey("dbo.DynamicFields", "Configuration_Id", "dbo.DynamicFieldTemplates");
            DropIndex("dbo.DocumentDynamicFields", new[] { "DynamicField_Id" });
            DropIndex("dbo.DocumentDynamicFields", new[] { "Document_Id" });
            DropIndex("dbo.ProjectDynamicFieldTemplates", new[] { "DynamicFieldTemplate_Id" });
            DropIndex("dbo.ProjectDynamicFieldTemplates", new[] { "Project_Id" });
            DropIndex("dbo.Documents", new[] { "Task_Id" });
            DropIndex("dbo.Tasks", new[] { "Project_Id" });
            DropIndex("dbo.Tasks", new[] { "CurrentUser_Id" });
            DropIndex("dbo.Users", new[] { "Team_Id" });
            DropIndex("dbo.Teams", new[] { "Project_Id" });
            DropIndex("dbo.DynamicFieldValues", new[] { "Id" });
            DropIndex("dbo.DynamicFields", new[] { "Project_Id" });
            DropIndex("dbo.DynamicFields", new[] { "Configuration_Id" });
            DropIndex("dbo.DynamicFieldTemplates", "IX_UQ_Code");
            DropTable("dbo.DocumentDynamicFields");
            DropTable("dbo.ProjectDynamicFieldTemplates");
            DropTable("dbo.Documents");
            DropTable("dbo.Tasks");
            DropTable("dbo.Users");
            DropTable("dbo.Teams");
            DropTable("dbo.DynamicFieldValues");
            DropTable("dbo.DynamicFields");
            DropTable("dbo.Projects");
            DropTable("dbo.DynamicFieldTemplates");
        }
    }
}
