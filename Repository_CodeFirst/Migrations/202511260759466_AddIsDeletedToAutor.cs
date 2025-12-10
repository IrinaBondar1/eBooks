namespace Repository_CodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsDeletedToAutor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Autor", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Autor", "IsDeleted");
        }
    }
}
