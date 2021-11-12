#nullable enable
namespace MoneyFox.Uwp.Views.Categories
{
    public sealed partial class AddCategoryDialog
    {
        public AddCategoryDialog()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.AddCategoryVm;
        }
    }
}