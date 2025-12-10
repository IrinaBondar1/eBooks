namespace Repository_CodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrare1_25_10_2025 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Autor",
                c => new
                    {
                        id_autor = c.Int(nullable: false, identity: true),
                        nume_autor = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.id_autor);
            
            CreateTable(
                "dbo.Carte",
                c => new
                    {
                        id_carte = c.Int(nullable: false, identity: true),
                        titlu = c.String(nullable: false, maxLength: 150),
                        descriere = c.String(maxLength: 500),
                        id_autor = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id_carte)
                .ForeignKey("dbo.Autor", t => t.id_autor, cascadeDelete: true)
                .Index(t => t.id_autor);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Carte", "id_autor", "dbo.Autor");
            DropIndex("dbo.Carte", new[] { "id_autor" });
            DropTable("dbo.Carte");
            DropTable("dbo.Autor");
        }
    }
}
