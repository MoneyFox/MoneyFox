using MoneyFox.DataAccess.Entities;

namespace MoneyFox.DataAccess.Pocos
{
    /// <summary>
    ///     Business object for group data.
    /// </summary>
    public class CategoryGroup
    {
        /// <summary>
        ///     Default constructor. Will Create a new CategoryGroupEntity
        /// </summary>
        public CategoryGroup()
        {
            Data = new CategoryGroupEntity();
        }

        /// <summary>
        ///     Set the data for this object.
        /// </summary>
        /// <param name="group">CategoryGroup Group data to wrap.</param>
        public CategoryGroup(CategoryGroupEntity group)
        {
            Data = group;
        }

        /// <summary>
        ///     CategoryGroup Data
        /// </summary>
        public CategoryGroupEntity Data { get; set; }
    }
}
