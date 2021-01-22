using MoneyFox.Uwp.ViewModels;
using System;
using Windows.UI.Xaml.Navigation;

#nullable enable
namespace MoneyFox.Uwp.Views.Categories
{
    public sealed partial class CategoryListView
    {
        public override bool ShowHeader => false;

        private CategoryListViewModel ViewModel => (CategoryListViewModel)DataContext;

        public CategoryListView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.CategoryListVm;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) => ViewModel.Subscribe();

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e) => ViewModel.Unsubscribe();

        private async void AddNewCategoryClick(object sender, Windows.UI.Xaml.RoutedEventArgs e) => await new AddCategoryDialog().ShowAsync();
    }
}
