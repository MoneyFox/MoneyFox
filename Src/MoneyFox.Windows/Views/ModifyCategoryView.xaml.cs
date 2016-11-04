using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace MoneyFox.Windows.Views
{
    /// <summary>
    /// Modify or create a CategoryViewModel
    /// </summary>
    public sealed partial class ModifyCategoryView
    {
        public ModifyCategoryView()
        {
            InitializeComponent();

            // code to handle bottom app bar when keyboard appears
            // workaround since otherwise the keyboard would overlay some controls
            InputPane.GetForCurrentView().Showing +=
                (s, args) => { BottomCommandBar.Visibility = Visibility.Collapsed; };
            InputPane.GetForCurrentView().Hiding += (s, args2) =>
            {
                if (BottomCommandBar.Visibility == Visibility.Collapsed)
                {
                    BottomCommandBar.Visibility = Visibility.Visible;
                }
            };
        }
    }
}
