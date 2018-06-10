using Windows.UI.Xaml;
using MoneyFox.Business.ViewModels;

namespace MoneyFox.Windows.Views
{
    public sealed partial class CategorySettingsView
    {
        public CategorySettingsView()
        {
            InitializeComponent();
        }

        private async void AddNew_OnClick(object sender, RoutedEventArgs e)
        {
            switch (CategoryPivot.SelectedIndex)
            {
                case 0:
                    await ((CategorySettingsViewModel) ViewModel)?.CategoryListViewModel.CreateNewCategoryCommand.ExecuteAsync();
                    break;

                case 1:
                    await ((CategorySettingsViewModel) ViewModel)?.CategoryGroupListViewModel.CreateNewGroupCommand.ExecuteAsync();
                    break;
            }
        }
    }
}