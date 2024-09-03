namespace WebApplication5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tablodüzenleme : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.aktivites",
                c => new
                    {
                        AktiviteID = c.Int(nullable: false, identity: true),
                        MusteriID = c.Int(nullable: false),
                        AktiviteTipi = c.String(nullable: false),
                        AktiviteTarihi = c.DateTime(nullable: false),
                        Aciklama = c.String(),
                    })
                .PrimaryKey(t => t.AktiviteID)
                .ForeignKey("dbo.Musteris", t => t.MusteriID, cascadeDelete: true)
                .Index(t => t.MusteriID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.aktivites", "MusteriID", "dbo.Musteris");
            DropIndex("dbo.aktivites", new[] { "MusteriID" });
            DropTable("dbo.aktivites");
        }
    }
}
