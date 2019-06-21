using System;
using Windows.UI.Xaml;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.Uwp.Views.Dialogs;
using MvvmCross;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class StatisticCategorySpreadingView
    {
        public StatisticCategorySpreadingView()
        {
            InitializeComponent();
        }

        private async void SetDate(object sender, RoutedEventArgs e)
        {
            if (Mvx.IoCProvider.CanResolve<SelectDateRangeDialogViewModel>())
            {
                await new SelectDateRangeDialog
                {
                    DataContext = Mvx.IoCProvider.Resolve<SelectDateRangeDialogViewModel>()
                }.ShowAsync();
            }
        }
    }
}