using System.Collections.Generic;
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
        ///     Searches all categories by the passed searchterm.
        ///     Only considers the Name field.
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        Task<IEnumerable<Category>> SearchByName(string searchTerm);
       
        /// <summary>
        ///     Save the passed category.
        /// </summary>
        Task SaveCategory(Category category);

        /// <summary>
        ///     Deletes the passed category and sets references to it to null.
        /// </summary>
        Task DeleteCategory(Category category);
    }

    /// <summary>
    ///     Offers service methods to access and modify category data.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IUnitOfWork unitOfWork;

        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            this.categoryRepository = categoryRepository;
            this.unitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await categoryRepository
                .GetAll()
                .OrderByName()
                .SelectCategories()
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Category>> SearchByName(string searchTerm)
        {
            return await categoryRepository
                .GetAll()
                .NameNotNull()
                .NameContains(searchTerm)
                .OrderByName()
                .SelectCategories()
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task SaveCategory(Category category)
        {
            categoryRepository.Add(category.Data);
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
