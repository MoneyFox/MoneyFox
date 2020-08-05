using MoneyFox.ViewModels.Categories;

namespace MoneyFox.Messages
{
    public class CategorySelectedMessage
    {
        public CategorySelectedMessage(CategoryViewModel selectedCategory)
        {
            SelectedCategory = selectedCategory;
        }

        public CategoryViewModel SelectedCategory { get; set; }
    }
}
