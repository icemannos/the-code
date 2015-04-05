namespace QuotationApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Quotations", "Quote", c => c.String(nullable: false));
            AlterColumn("dbo.Quotations", "Author", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Quotations", "Author", c => c.String());
            AlterColumn("dbo.Quotations", "Quote", c => c.String());
        }
    }
}
