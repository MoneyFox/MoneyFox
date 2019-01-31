using Windows.System;
using Windows.UI.Xaml.Input;
using MoneyFox.ServiceLayer.ViewModels;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class SelectCategoryListView
    {
        public SelectCategoryListView()
        {
            InitializeComponent();
        }

        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                var viewModel = (SelectCategoryListViewModel) DataContext;
                viewModel.ItemClickCommand.Execute(viewModel.SelectedCategory);
            }

            base.OnKeyDown(e);
        }
    }
}