namespace MehmetBirolGolge_231502010.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 50),
                        Color = c.String(maxLength: 7),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.TaskCategories",
                c => new
                    {
                        TaskID = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TaskID, t.CategoryID })
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.Tasks", t => t.TaskID, cascadeDelete: true)
                .Index(t => t.TaskID)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        TaskID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 200),
                        Description = c.String(),
                        DueDate = c.DateTime(),
                        IsCompleted = c.Boolean(nullable: false),
                        Priority = c.String(maxLength: 20),
                        AIDescription = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TaskID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 100),
                        Role = c.String(maxLength: 20),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskCategories", "TaskID", "dbo.Tasks");
            DropForeignKey("dbo.Tasks", "UserID", "dbo.Users");
            DropForeignKey("dbo.TaskCategories", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Tasks", new[] { "UserID" });
            DropIndex("dbo.TaskCategories", new[] { "CategoryID" });
            DropIndex("dbo.TaskCategories", new[] { "TaskID" });
            DropTable("dbo.Users");
            DropTable("dbo.Tasks");
            DropTable("dbo.TaskCategories");
            DropTable("dbo.Categories");
        }
    }
}
