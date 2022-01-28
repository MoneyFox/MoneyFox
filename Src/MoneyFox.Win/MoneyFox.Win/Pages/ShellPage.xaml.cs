using Microsoft.UI.Xaml.Controls;
using MoneyFox.Win.ViewModels;

namespace MoneyFox.Win.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShellPage : Page
    {
        private ShellViewModel ViewModel => (ShellViewModel)DataContext;

        public ShellPage()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.ShellVm;
        }

        private void AddPaymentItemTapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {

        }
    }
}
