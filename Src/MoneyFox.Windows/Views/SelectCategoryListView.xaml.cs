using Windows.System;
using Windows.UI.Xaml.Input;
using MoneyFox.Business.ViewModels;

namespace MoneyFox.Windows.Views
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
                SelectCategoryListViewModel viewModel = (SelectCategoryListViewModel) DataContext;
                viewModel.ItemClickCommand.Execute(viewModel.SelectedCategory);
            }

            base.OnKeyDown(e);
        }
    }
}