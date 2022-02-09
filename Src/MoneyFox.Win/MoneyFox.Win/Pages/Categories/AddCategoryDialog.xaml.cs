namespace MoneyFox.Win.Pages.Categories
{
    public sealed partial class AddCategoryDialog
    {
        public AddCategoryDialog()
        {
            XamlRoot = MainWindow.RootFrame.XamlRoot;
            InitializeComponent();
            DataContext = ViewModelLocator.AddCategoryVm;
        }
    }
}