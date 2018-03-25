using Windows.UI.Xaml;
using MoneyFox.Views;
using Xamarin.Forms.Platform.UWP;

namespace MoneyFox.Windows.Views
{
    public sealed partial class StatisticSelectorView
    {
        public StatisticSelectorView()
        {
            InitializeComponent();
        }

        private void StatisticSelectorView_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ContentGrid.Children.Clear();
            ContentGrid.Children.Add(new StatisticSelectorPage { DataContext = ViewModel }.CreateFrameworkElement());
        }
    }
}