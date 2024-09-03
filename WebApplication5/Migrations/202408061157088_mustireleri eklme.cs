namespace WebApplication5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mustirelerieklme : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Musteris",
                c => new
                    {
                        MusteriID = c.Int(nullable: false, identity: true),
                        AdSoyad = c.String(),
                        Emaıl = c.Int(nullable: false),
                        Telefon = c.Int(nullable: false),
                        KayıtTarihi = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MusteriID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Musteris");
        }
    }
}
