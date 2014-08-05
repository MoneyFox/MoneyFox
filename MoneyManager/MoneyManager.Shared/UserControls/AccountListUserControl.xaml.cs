using MoneyManager.Models;
using MoneyManager.Src;
using MoneyManager.ViewModels;
using MoneyManager.ViewModels.Data;
using MoneyManager.Views;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace MoneyManager.UserControls
{
    public sealed partial class AccountListUserControl
    {
        public AccountListUserControl()
        {
            InitializeComponent();
        }

        private void AccountList_Holding(object sender, HoldingRoutedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);
        }
    }
}