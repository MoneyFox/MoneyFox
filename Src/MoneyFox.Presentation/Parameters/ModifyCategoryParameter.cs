namespace MoneyFox.Presentation.Parameters
{
    /// <summary>
    ///     Parameter object for the ModifyCategoryView.
    /// </summary>
    public class ModifyCategoryParameter
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="categoryId">Category Id to edit</param>
        public ModifyCategoryParameter(int categoryId = 0)
        {
            CategoryId = categoryId;
        }

        /// <summary>
        ///     Category Id who shall be edited.
        ///     If this is 0, a new category shall be created.
        /// </summary>
        public int CategoryId { get; set; }
    }
}
