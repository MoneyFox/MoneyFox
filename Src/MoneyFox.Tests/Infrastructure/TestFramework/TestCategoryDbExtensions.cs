namespace MoneyFox.Tests.Infrastructure.TestFramework
{

    using MoneyFox.Infrastructure.Persistence;

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
