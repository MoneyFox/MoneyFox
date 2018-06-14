using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.DataAccess.QueryExtensions;

namespace MoneyFox.DataAccess.DataServices
{
    /// <summary>
    ///     Offers service methods to access and modify category data.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        ///     Returns all categories
        /// </summary>
        /// <returns>List with all categories.</returns>
        Task<IEnumerable<Category>> GetAllCategories();

        /// <summary>
        ///     Returns all categories and joins the payments to them.
        /// </summary>
        /// <returns>List with all categories with payments.</returns>
        Task<IEnumerable<Category>> GetAllCategoriesWithPayments();

        /// <summary>
        ///     Looks up the category with the passed id and returns it.
        /// </summary>
        /// <param name="id">Id to look for.</param>
        /// <returns>Found Id.</returns>
        Task<Category> GetById(int id);

        /// <summary>
        ///     Searches all categories by the passed searchterm.
        ///     Only considers the Name field.
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        Task<IEnumerable<Category>> SearchByName(string searchTerm);

        /// <summary>
        ///     Checks if the name is already taken by another category.
        /// </summary>
        /// <param name="name">Name to look for.</param>
        /// <returns>If category name is already taken.</returns>
        Task<bool> CheckIfNameAlreadyTaken(string name);

        /// <summary>
        ///     Save the passed category.
        /// </summary>
        /// <param name="category">Category to save.</param>
        Task SaveCategory(Category category);

        /// <summary>
        ///     Deletes the passed category and sets references to it to null.
        /// </summary>
        /// <param name="category">Category to delete.</param>
        Task DeleteCategory(Category category);
    }

    /// <summary>
    ///     Offers service methods to access and modify category data.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly IAmbientDbContextLocator ambientDbContextLocator;
        private readonly IDbContextScopeFactory dbContextScopeFactory;

        /// <summary>
        ///     Constructor
        /// </summary>
        public CategoryService(IAmbientDbContextLocator ambientDbContextLocator, IDbContextScopeFactory dbContextScopeFactory)
        {
            this.ambientDbContextLocator = ambientDbContextLocator;
            this.dbContextScopeFactory = dbContextScopeFactory;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    return await dbContext.Categories
                                              .OrderByName()
                                              .SelectCategories()
                                              .ToListAsync();
                }
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Category>> GetAllCategoriesWithPayments()
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    var list = await dbContext.Categories
                        .Include(x => x.Payments)
                        .OrderByName()
                        .ToListAsync();

                    return list.Select(x => new Category(x));
                }
            }
        }

        /// <inheritdoc />
        public async Task<Category> GetById(int id)
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    var category = await dbContext.Categories.FindAsync(id);
                    return category == null ? null : new Category(category);
                }
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Category>> SearchByName(string searchTerm)
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    var list = await dbContext.Categories
                        .NameNotNull()
                        .NameContains(searchTerm)
                        .OrderByName()
                        .ToListAsync();

                    return list.Select(x => new Category(x));
                }
            }
        }

        /// <inheritdoc />
        public async Task<bool> CheckIfNameAlreadyTaken(string name)
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    return await dbContext.Categories
                                          .NameEquals(name)
                                          .AnyAsync();
                }
            }
        }

        /// <inheritdoc />
        public async Task SaveCategory(Category category)
        {
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    if (category.Data.Id == 0)
                    {
                        dbContext.Entry(category.Data).State = EntityState.Added;
                    }
                    else
                    {
                        dbContext.Entry(category.Data).State = EntityState.Modified;
                    }
                    await dbContextScope.SaveChangesAsync();
                }
            }
        }

        /// <inheritdoc />
        public async Task DeleteCategory(Category category)
        {
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    dbContext.Entry(category.Data).State = EntityState.Deleted;
                    await dbContextScope.SaveChangesAsync();
                }
            }
        }
    }
}