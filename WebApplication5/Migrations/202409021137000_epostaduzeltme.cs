namespace WebApplication5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class epostaduzeltme : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Eposta", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Eposta");
        }
    }
}
