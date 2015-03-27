namespace QuotationApp1.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<QuotationApp1.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "QuotationApp1.Models.ApplicationDbContext";
        }

        protected override void Seed(QuotationApp1.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            if (context.Categories.Count() == 0)
            {
                context.Categories.Add(new Category { Name = "Funny" });
                context.Categories.Add(new Category { Name = "Wisdom" });
                context.Categories.Add(new Category { Name = "Motivational" });
                context.Categories.Add(new Category { Name = "Inspirational" });
                context.SaveChanges();
            }
        }
    }
}
