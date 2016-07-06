using System;
using Windows.UI.Xaml;
using MoneyFox.Shared.ViewModels;
using MoneyFox.Windows.Views.Dialogs;
using MvvmCross.Platform;

namespace MoneyFox.Windows.Views
{
    public sealed partial class CategoryListView
    {
        public CategoryListView()
        {
            InitializeComponent();
            CategoryListUserControl.DataContext = Mvx.Resolve<CategoryListViewModel>();
        }
    }
}