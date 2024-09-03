namespace WebApplication5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Düzeltme : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Musteris", "Emaıl", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Musteris", "Emaıl", c => c.Int(nullable: false));
        }
    }
}
