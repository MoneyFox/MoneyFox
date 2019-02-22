using System;
using Windows.UI.Xaml;
using MoneyFox.Presentation.Views;
using MoneyFox.ServiceLayer.ViewModels;
using MoneyFox.Uwp.Views.Dialogs;
using MvvmCross;
using Xamarin.Forms.Platform.UWP;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class StatisticCategorySummaryView
    {
        public StatisticCategorySummaryView()
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

        private void StatisticCategorySummaryView_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ContentGrid.Children.Clear();
            ContentGrid.Children.Add(new StatisticCategorySummaryPage() { DataContext = ViewModel }.CreateFrameworkElement());
        }
    }
}