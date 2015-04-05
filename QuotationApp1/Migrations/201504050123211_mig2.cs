namespace QuotationApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Quotations", "CreateCategory", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Quotations", "CreateCategory");
        }
    }
}
