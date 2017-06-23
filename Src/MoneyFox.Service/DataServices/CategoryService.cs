using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Service.Pocos;
using MoneyFox.Service.QueryExtensions;

namespace MoneyFox.Service.DataServices
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
        private readonly ICategoryRepository categoryRepository;
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        ///     Constructor
        /// </summary>
        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            this.categoryRepository = categoryRepository;
            this.unitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            var list = await categoryRepository
                .GetAll()
                .OrderByName()
                .ToListAsync();

            return list.Select(x => new Category(x));
        }

        /// <inheritdoc />
        public async Task<Category> GetById(int id)
        {
            return new Category(await categoryRepository.GetById(id));
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Category>> SearchByName(string searchTerm)
        {
            var list = await categoryRepository
                .GetAll()
                .NameNotNull()
                .NameContains(searchTerm)
                .OrderByName()
                .ToListAsync();

            return list.Select(x => new Category(x));
        }

        /// <inheritdoc />
        public Task<bool> CheckIfNameAlreadyTaken(string name)
        {
            return categoryRepository.GetAll()
                                    .NameEquals(name)
                                    .AnyAsync();
        }

        /// <inheritdoc />
        public async Task SaveCategory(Category category)
        {
            if (category.Data.Id == 0)
            {
                categoryRepository.Add(category.Data);
            } else
            {
                categoryRepository.Update(category.Data);
            }
            await unitOfWork.Commit();
        }

        /// <inheritdoc />
        public async Task DeleteCategory(Category category)
        {
            categoryRepository.Delete(category.Data);
            await unitOfWork.Commit();
        }
    }
}
