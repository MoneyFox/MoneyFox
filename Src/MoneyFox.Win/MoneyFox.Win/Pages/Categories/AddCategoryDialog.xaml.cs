namespace MoneyFox.Win.Pages.Categories
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