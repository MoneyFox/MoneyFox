using Windows.System;
using Windows.UI.Xaml.Input;
using MoneyFox.Business.ViewModels;
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

        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                SelectCategoryListViewModel viewModel = (SelectCategoryListViewModel) DataContext;
                viewModel.SelectCommand.Execute(viewModel.SelectedCategory);
            }

            base.OnKeyDown(e);
        }
    }
}