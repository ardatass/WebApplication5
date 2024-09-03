namespace WebApplication5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class telefondüzenleme : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Musteris", "Telefon", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Musteris", "Telefon", c => c.Int(nullable: false));
        }
    }
}
