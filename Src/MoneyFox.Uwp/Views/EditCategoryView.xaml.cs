using Windows.UI.Xaml.Navigation;
using MoneyFox.Uwp.ViewModels;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class EditCategoryView
    {
        public override string Header => ViewModelLocator.EditCategoryVm.Title;

        private EditCategoryViewModel ViewModel => DataContext as EditCategoryViewModel;

        public EditCategoryView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null) ViewModel.CategoryId = (int) e.Parameter;
        }
    }
}
