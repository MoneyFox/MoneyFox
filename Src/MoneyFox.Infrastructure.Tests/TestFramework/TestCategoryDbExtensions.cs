namespace MoneyFox.Infrastructure.Tests.TestFramework
{

    using Persistence;

    internal static class TestCategoryDbExtensions
    {
        public static void RegisterCategories(this AppDbContext db, params TestData.ICategory[] categories)
        {
            foreach (var testCategories in categories)
            {
                db.Add(testCategories.CreateDbCategory());
            }

            db.SaveChanges();
        }
    }

}
