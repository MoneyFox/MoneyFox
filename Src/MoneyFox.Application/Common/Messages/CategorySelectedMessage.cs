namespace MoneyFox.Application.Common.Messages
{
    /// <summary>
    ///     Used to notify other view models about a selected category.
    /// </summary>
    public class CategorySelectedMessage
    {
        /// <summary>
        ///     Message to notify about a selected CategoryViewModel after choosing.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="selectedCategoryId">Id of the Selected Category</param>
        public CategorySelectedMessage(object sender, int selectedCategoryId)
        {
            SelectedCategoryId = selectedCategoryId;
        }

        /// <summary>
        ///     Selected CategoryViewModel.
        /// </summary>
        public int SelectedCategoryId { get; }
    }
}
