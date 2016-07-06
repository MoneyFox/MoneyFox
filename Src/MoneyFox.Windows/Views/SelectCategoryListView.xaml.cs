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