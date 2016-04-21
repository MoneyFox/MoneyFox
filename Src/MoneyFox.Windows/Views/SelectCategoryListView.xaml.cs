using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using MoneyFox.Shared.ViewModels;
using MoneyFox.Windows.Views.Dialogs;
using MvvmCross.Platform;

namespace MoneyFox.Windows.Views
{
    public sealed partial class SelectCategoryListView
    {
        public SelectCategoryListView()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<SelectCategoryListViewModel>();
        }

        private async void AddCategory(object sender, RoutedEventArgs e)
        {
            await new ModifyCategoryDialog().ShowAsync();

            var selectCategoryListViewModel = DataContext as SelectCategoryListViewModel;
            if (selectCategoryListViewModel != null)
            {
                selectCategoryListViewModel.SearchText = string.Empty;
                selectCategoryListViewModel.Search();
            }
        }

        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                ((SelectCategoryListViewModel) DataContext).DoneCommand.Execute(null);
            }

            base.OnKeyDown(e);
        }
    }
}