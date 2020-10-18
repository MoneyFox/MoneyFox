using Windows.UI.Xaml.Controls;

#nullable enable
namespace MoneyFox.Uwp.Views
{
    public class BaseView : Page
    {
        public virtual string Header => string.Empty;
        public virtual bool ShowHeader => true;
    }
}
