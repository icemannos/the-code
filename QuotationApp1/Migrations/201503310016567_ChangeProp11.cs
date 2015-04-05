namespace QuotationApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeProp11 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Quotations", "Quote", c => c.String());
            AlterColumn("dbo.Quotations", "Author", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Quotations", "Author", c => c.String(nullable: false));
            AlterColumn("dbo.Quotations", "Quote", c => c.String(nullable: false));
        }
    }
}
