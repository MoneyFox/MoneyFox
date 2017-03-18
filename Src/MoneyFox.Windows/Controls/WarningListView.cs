using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using MoneyFox.Foundation.DataModels;

namespace MoneyFox.Windows.Controls
{
    public class WarningListView : ListView
    {
        public static readonly DependencyProperty WarningBackgroundProperty =
            DependencyProperty.Register("WarningBackground", typeof(Brush), typeof(AlternatingRowListView), null);
      

        public Brush WarningBackground
        {
            get { return (Brush)GetValue(WarningBackgroundProperty); }
            set { SetValue(WarningBackgroundProperty, value); }
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            var listViewItem = element as ListViewItem;
            var accountItem = item as AccountViewModel;
            if (listViewItem != null)
            {
                if (accountItem != null && accountItem.IsOverdrawn)
                {
                    listViewItem.Background = WarningBackground;
                }
            }
        }
    }
}