using MoneyFox.Foundation.DataModels;
using MvvmCross.Plugins.Messenger;

namespace MoneyFox.Foundation.Messages
{
    public class CategorySelectedMessage : MvxMessage
    {
        /// <summary>
        ///     Message to notify about a selected CategoryViewModel after choosing.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="selectedCategory">Selected CategoryViewModel</param>
        public CategorySelectedMessage(object sender, CategoryViewModel selectedCategory) : base(sender)
        {
            SelectedCategory = selectedCategory;
        }

        /// <summary>
        ///     Selected CategoryViewModel.
        /// </summary>
        public CategoryViewModel SelectedCategory { get; }
    }
}