using System;
using Windows.UI.Xaml;
using MoneyFox.Windows.Views.Dialogs;
using MvvmCross;
using MoneyFox.Business.ViewModels;

namespace MoneyFox.Windows.Views
{
    public sealed partial class StatisticCategorySpreadingView
    {
        public StatisticCategorySpreadingView()
        {
            InitializeComponent();
        }

        private async void SetDate(object sender, RoutedEventArgs e)
        {
            await new SelectDateRangeDialog { DataContext = Mvx.Resolve<SelectDateRangeDialogViewModel>() }.ShowAsync();
        }
    }
}