using MoneyFox.Presentation.ViewModels;

namespace MoneyFox.Presentation.Messages
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
        /// <param name="selectedCategory">Selected CategoryViewModel</param>
        public CategorySelectedMessage(object sender, CategoryViewModel selectedCategory)
        {
            SelectedCategory = selectedCategory;
        }

        /// <summary>
        ///     Selected CategoryViewModel.
        /// </summary>
        public CategoryViewModel SelectedCategory { get; }
    }
}
