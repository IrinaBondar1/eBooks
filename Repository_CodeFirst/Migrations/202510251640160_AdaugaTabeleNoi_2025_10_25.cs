namespace Repository_CodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdaugaTabeleNoi_2025_10_25 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categorie",
                c => new
                    {
                        id_categorie = c.Int(nullable: false, identity: true),
                        denumire = c.String(nullable: false, maxLength: 100),
                        descriere = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.id_categorie);
            
            CreateTable(
                "dbo.IstoricCitire",
                c => new
                    {
                        id_istoric = c.Int(nullable: false, identity: true),
                        id_utilizator = c.Int(nullable: false),
                        id_carte = c.Int(nullable: false),
                        data_accesare = c.DateTime(nullable: false),
                        actiune = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.id_istoric)
                .ForeignKey("dbo.Carte", t => t.id_carte, cascadeDelete: true)
                .ForeignKey("dbo.Utilizator", t => t.id_utilizator, cascadeDelete: true)
                .Index(t => t.id_utilizator)
                .Index(t => t.id_carte);
            
            CreateTable(
                "dbo.Utilizator",
                c => new
                    {
                        id_utilizator = c.Int(nullable: false, identity: true),
                        nume_utilizator = c.String(nullable: false, maxLength: 100),
                        email = c.String(nullable: false, maxLength: 100),
                        parola = c.String(nullable: false, maxLength: 100),
                        data_inregistrare = c.DateTime(nullable: false),
                        id_tip_abonament = c.Int(nullable: false),
                        carti_citite_luna = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id_utilizator)
                .ForeignKey("dbo.TipAbonament", t => t.id_tip_abonament, cascadeDelete: true)
                .Index(t => t.id_tip_abonament);
            
            CreateTable(
                "dbo.TipAbonament",
                c => new
                    {
                        id_tip_abonament = c.Int(nullable: false, identity: true),
                        denumire = c.String(nullable: false, maxLength: 100),
                        limita_carti_pe_luna = c.Int(nullable: false),
                        acces_serii_complete = c.Boolean(nullable: false),
                        permite_descarcare = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id_tip_abonament);
            
            CreateTable(
                "dbo.Serie",
                c => new
                    {
                        id_serie = c.Int(nullable: false, identity: true),
                        nume_serie = c.String(nullable: false, maxLength: 100),
                        descriere = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.id_serie);
            
            AddColumn("dbo.Carte", "Categorie_id_categorie", c => c.Int());
            AddColumn("dbo.Carte", "Serie_id_serie", c => c.Int());
            CreateIndex("dbo.Carte", "Categorie_id_categorie");
            CreateIndex("dbo.Carte", "Serie_id_serie");
            AddForeignKey("dbo.Carte", "Categorie_id_categorie", "dbo.Categorie", "id_categorie");
            AddForeignKey("dbo.Carte", "Serie_id_serie", "dbo.Serie", "id_serie");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Carte", "Serie_id_serie", "dbo.Serie");
            DropForeignKey("dbo.IstoricCitire", "id_utilizator", "dbo.Utilizator");
            DropForeignKey("dbo.Utilizator", "id_tip_abonament", "dbo.TipAbonament");
            DropForeignKey("dbo.IstoricCitire", "id_carte", "dbo.Carte");
            DropForeignKey("dbo.Carte", "Categorie_id_categorie", "dbo.Categorie");
            DropIndex("dbo.Utilizator", new[] { "id_tip_abonament" });
            DropIndex("dbo.IstoricCitire", new[] { "id_carte" });
            DropIndex("dbo.IstoricCitire", new[] { "id_utilizator" });
            DropIndex("dbo.Carte", new[] { "Serie_id_serie" });
            DropIndex("dbo.Carte", new[] { "Categorie_id_categorie" });
            DropColumn("dbo.Carte", "Serie_id_serie");
            DropColumn("dbo.Carte", "Categorie_id_categorie");
            DropTable("dbo.Serie");
            DropTable("dbo.TipAbonament");
            DropTable("dbo.Utilizator");
            DropTable("dbo.IstoricCitire");
            DropTable("dbo.Categorie");
        }
    }
}
