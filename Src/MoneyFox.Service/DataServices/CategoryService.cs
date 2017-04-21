using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
    }

    /// <summary>
    ///     Offers service methods to access and modify category data.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await categoryRepository.GetAll().SelectCategories().ToListAsync();
        }
    }
}
