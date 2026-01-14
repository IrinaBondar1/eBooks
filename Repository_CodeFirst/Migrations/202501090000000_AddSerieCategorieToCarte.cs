namespace Repository_CodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSerieCategorieToCarte : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Carte", "id_categorie", c => c.Int());
            AddColumn("dbo.Carte", "id_serie", c => c.Int());
            AddColumn("dbo.Carte", "nr_volum", c => c.Int());
            CreateIndex("dbo.Carte", "id_categorie");
            CreateIndex("dbo.Carte", "id_serie");
            AddForeignKey("dbo.Carte", "id_categorie", "dbo.Categorie", "id_categorie");
            AddForeignKey("dbo.Carte", "id_serie", "dbo.Serie", "id_serie");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Carte", "id_serie", "dbo.Serie");
            DropForeignKey("dbo.Carte", "id_categorie", "dbo.Categorie");
            DropIndex("dbo.Carte", new[] { "id_serie" });
            DropIndex("dbo.Carte", new[] { "id_categorie" });
            DropColumn("dbo.Carte", "nr_volum");
            DropColumn("dbo.Carte", "id_serie");
            DropColumn("dbo.Carte", "id_categorie");
        }
    }
}
