using MoneyFox.DataAccess.Entities;

namespace MoneyFox.DataAccess.Pocos
{
    /// <summary>
    ///     Business object for category data.
    /// </summary>
    public class Category
    {
        /// <summary>
        ///     Default constructor. Will Create a new CategoryEntity
        /// </summary>
        public Category()
        {
            Data = new CategoryEntity();
        }

        /// <summary>
        ///     Set the data for this object.
        /// </summary>
        /// <param name="category">Category data to wrap.</param>
        public Category(CategoryEntity category)
        {
            Data = category;
        }

        /// <summary>
        ///     Categorydata
        /// </summary>
        public CategoryEntity Data { get; set; }
    }
}
