namespace NotOurStackOverflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChosenToAnswers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "IsAccepted", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "IsAccepted");
        }
    }
}
