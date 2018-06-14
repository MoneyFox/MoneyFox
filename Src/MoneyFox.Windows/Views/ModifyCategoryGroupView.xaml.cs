using Windows.UI.Xaml;
using MoneyFox.Views;
using Xamarin.Forms.Platform.UWP;

namespace MoneyFox.Windows.Views
{
    public sealed partial class ModifyCategoryGroupView
    {
        public ModifyCategoryGroupView()
        {
            this.InitializeComponent();
        }

        private void ModifyGroup_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ContentGrid.Children.Clear();
            ContentGrid.Children.Add(new ModifyCategoryGroupPage { DataContext = ViewModel }.CreateFrameworkElement());
        }
    }
}
