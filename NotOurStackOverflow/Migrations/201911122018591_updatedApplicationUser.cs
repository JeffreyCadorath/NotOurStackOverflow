namespace NotOurStackOverflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedApplicationUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Posts", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Posts", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropIndex("dbo.Posts", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Posts", new[] { "ApplicationUser_Id1" });
            DropColumn("dbo.Posts", "ApplicationUser_Id");
            DropColumn("dbo.Posts", "ApplicationUser_Id1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "ApplicationUser_Id1", c => c.String(maxLength: 128));
            AddColumn("dbo.Posts", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Posts", "ApplicationUser_Id1");
            CreateIndex("dbo.Posts", "ApplicationUser_Id");
            AddForeignKey("dbo.Posts", "ApplicationUser_Id1", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Posts", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
