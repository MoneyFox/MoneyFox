using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MoneyFox.Windows.Views
{
    /// <summary>
    ///     The basic frame for the windows app
    /// </summary>
    public sealed partial class ShellPage : Page
    {
        public Frame Frame => ShellFrame;

        public ShellViewModel ViewModel { get; set; }

        public ShellPage()
        {
            InitializeComponent();
        }

        private void ShellPage_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // we have to update the color here, since it is not a dependency property.
            NavigationMenu.PaneForeground = ViewModel.MenuButtonColor;
        }
    }
}
