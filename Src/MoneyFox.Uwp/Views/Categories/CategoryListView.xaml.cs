using MoneyFox.Application.Resources;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.ViewModels.Categories;
using System;
using Windows.UI.Xaml.Navigation;

#nullable enable
namespace MoneyFox.Uwp.Views.Categories
{
    public sealed partial class CategoryListView
    {
        public override string Header => Strings.CategoriesTitle;

        private CategoryListViewModel ViewModel => (CategoryListViewModel)DataContext;

        public CategoryListView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.CategoryListVm;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
            => ViewModel.Subscribe();

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
            => ViewModel.Unsubscribe();

        private async void AddNewCategoryClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
            => await new AddCategoryDialog{RequestedTheme = ThemeSelectorService.Theme}.ShowAsync();
    }
}
