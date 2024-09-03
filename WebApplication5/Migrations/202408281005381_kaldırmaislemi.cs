namespace WebApplication5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kaldırmaislemi : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.aktivites", "AdSoyad");
        }
        
        public override void Down()
        {
            AddColumn("dbo.aktivites", "AdSoyad", c => c.String(nullable: false));
        }
    }
}
