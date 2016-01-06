using MoneyManager.Foundation.Model;
using MvvmCross.Plugins.Messenger;

namespace MoneyManager.Foundation.Messages
{
    public class CategorySelectedMessage : MvxMessage
    {
        public CategorySelectedMessage(object sender, Category selectedCategory) : base(sender)
        {
            SelectedCategory = selectedCategory;
        }

        public Category SelectedCategory { get; private set; }
    }
}