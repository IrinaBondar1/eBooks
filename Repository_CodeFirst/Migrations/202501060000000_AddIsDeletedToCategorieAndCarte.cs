namespace Repository_CodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsDeletedToCategorieAndCarte : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categorie", "IsDeleted", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.Carte", "IsDeleted", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Carte", "IsDeleted");
            DropColumn("dbo.Categorie", "IsDeleted");
        }
    }
}

