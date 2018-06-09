using System.Collections.Generic;
using System.Threading.Tasks;
using EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.DataAccess.QueryExtensions;

namespace MoneyFox.DataAccess.DataServices
{
    public interface ICategoryGroupService
    {
        /// <summary>
        ///     Returns all groups
        /// </summary>
        /// <returns>List with all categories.</returns>
        Task<IEnumerable<CategoryGroup>> GetAllGroups();

        /// <summary>
        ///     Looks up the groups with the passed id and returns it.
        /// </summary>
        /// <param name="id">Id to look for.</param>
        /// <returns>Found Id.</returns>
        Task<CategoryGroup> GetById(int id);

        /// <summary>
        ///     Checks if the name is already taken by another group.
        /// </summary>
        /// <param name="name">Name to look for.</param>
        /// <returns>If category name is already taken.</returns>
        Task<bool> CheckIfNameAlreadyTaken(string name);

        /// <summary>
        ///     Save the passed group.
        /// </summary>
        /// <param name="categoryGroup">Group to save.</param>
        Task SaveGroup(CategoryGroup categoryGroup);

        /// <summary>
        ///     Deletes the passed group and sets references to it to null.
        /// </summary>
        /// <param name="categoryGroup">group to delete.</param>
        Task DeleteGroup(CategoryGroup categoryGroup);
    }

    /// <inheritdoc />
    public class CategoryGroupService : ICategoryGroupService
    {
        private readonly IAmbientDbContextLocator ambientDbContextLocator;
        private readonly IDbContextScopeFactory dbContextScopeFactory;

        public CategoryGroupService(IAmbientDbContextLocator ambientDbContextLocator, IDbContextScopeFactory dbContextScopeFactory)
        {
            this.ambientDbContextLocator = ambientDbContextLocator;
            this.dbContextScopeFactory = dbContextScopeFactory;
        }


        /// <inheritdoc />
        public async Task<IEnumerable<CategoryGroup>> GetAllGroups()
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    return await dbContext.Groups
                                          .OrderByName()
                                          .SelectGroup()
                                          .ToListAsync();
                }
            }
        }

        /// <inheritdoc />
        public async Task<CategoryGroup> GetById(int id)
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    var group = await dbContext.Groups.FindAsync(id);
                    return group == null ? null : new CategoryGroup(group);
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
                    return await dbContext.Groups
                                          .NameEquals(name)
                                          .AnyAsync();
                }
            }
        }


        /// <inheritdoc />
        public async Task SaveGroup(CategoryGroup categoryGroup)
        {
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    if (categoryGroup.Data.Id == 0)
                    {
                        dbContext.Entry(categoryGroup.Data).State = EntityState.Added;
                    } else
                    {
                        dbContext.Entry(categoryGroup.Data).State = EntityState.Modified;
                    }
                    await dbContextScope.SaveChangesAsync();
                }
            }
        }

        /// <inheritdoc />
        public async Task DeleteGroup(CategoryGroup categoryGroup)
        {
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    dbContext.Entry(categoryGroup.Data).State = EntityState.Deleted;
                    await dbContextScope.SaveChangesAsync();
                }
            }
        }
    }
}
