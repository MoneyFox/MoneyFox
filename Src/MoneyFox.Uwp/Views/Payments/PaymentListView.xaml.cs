﻿using Microsoft.Toolkit.Uwp.UI.Controls;
using MoneyFox.Uwp.ViewModels.Payments;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

#nullable enable
namespace MoneyFox.Uwp.Views.Payments
{
    public sealed partial class PaymentListView
    {
        public override bool ShowHeader => false;

        private PaymentListViewModel ViewModel => (PaymentListViewModel)DataContext;

        public PaymentListView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.PaymentListVm;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter != null)
            {
                ViewModel.AccountId = (int)e.Parameter;
            }
        }

        private void OpenFilterFlyout(object sender, RoutedEventArgs e) =>
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);

        private void DataGrid_LoadingRowGroup(object sender, DataGridRowGroupHeaderEventArgs e)
        {
            ICollectionViewGroup group = e.RowGroupHeader.CollectionViewGroup;
            var item = (PaymentViewModel)group.GroupItems[0];
            e.RowGroupHeader.PropertyValue = item.Date.ToString("D", CultureInfo.CurrentCulture);
        }

        private void DataGrid_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if(((FrameworkElement)e.OriginalSource).DataContext is PaymentViewModel vm)
            {
                ViewModel.EditPaymentCommand.Execute(vm);
            }
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            if(((FrameworkElement)sender).DataContext is PaymentViewModel vm)
            {
                ViewModel.DeletePaymentCommand.Execute(vm);
            }
        }
    }
}