using MoneyFox.Domain;
using System;
using Windows.UI.Xaml.Controls;

namespace MoneyFox.Uwp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppShell
    {
        private WindowsShellViewModel ViewModel => (WindowsShellViewModel) DataContext;

        public AppShell()
        {
            InitializeComponent();
            DataContext = new WindowsShellViewModel();

            ViewModel.Initialize(ContentFrame, NavView, KeyboardAccelerators);
        }

        public Frame MainFrame => ContentFrame;

        private async void AddPaymentItemTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            await new AddPaymentDialog(PaymentType.Expense).ShowAsync();
        }
    }
}
