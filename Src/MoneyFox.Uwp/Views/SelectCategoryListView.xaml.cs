using MoneyFox.Application.Resources;
using MoneyFox.Uwp.ViewModels;
using Windows.System;
using Windows.UI.Xaml.Input;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class SelectCategoryListView
    {
        public override string Header => Strings.SelectCategoryTitle;

        private SelectCategoryListViewModel ViewModel => (SelectCategoryListViewModel) DataContext;

        public SelectCategoryListView()
        {
            InitializeComponent();
        }

        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            if(e.Key == VirtualKey.Enter)
            {
                SelectCategoryListViewModel viewModel = ViewModel;
                viewModel.ItemClickCommand.Execute(viewModel.SelectedCategory);
            }

            base.OnKeyDown(e);
        }
    }
}
