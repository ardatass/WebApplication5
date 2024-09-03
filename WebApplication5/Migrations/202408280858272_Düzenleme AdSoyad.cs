namespace WebApplication5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DüzenlemeAdSoyad : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.aktivites", "AdSoyad", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.aktivites", "AdSoyad");
        }
    }
}
