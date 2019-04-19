using Microsoft.UI.Xaml.Controls;
using MoneyFox.ServiceLayer.ViewModels;

namespace MoneyFox.Windows.Views
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShellView
    {
        public ShellView()
        {
            InitializeComponent();
            DataContext = new ShellViewModel();
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
        }
    }
}