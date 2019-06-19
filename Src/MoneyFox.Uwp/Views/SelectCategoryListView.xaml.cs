using Windows.System;
using Windows.UI.Xaml.Input;
using MoneyFox.Presentation.ViewModels;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class SelectCategoryListView
    {
        private SelectCategoryListViewModel ViewModel => (SelectCategoryListViewModel) DataContext;

        public SelectCategoryListView()
        {
            InitializeComponent();
        }

        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                var viewModel = ViewModel;
                viewModel.ItemClickCommand.Execute(viewModel.SelectedCategory);
            }

            base.OnKeyDown(e);
        }
    }
}