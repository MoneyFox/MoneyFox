using Microsoft.UI.Xaml.Controls;

namespace MoneyFox.Win.Pages
{
    public class BasePage : Page
    {
        public virtual string Header => string.Empty;
        public virtual bool ShowHeader => true;
    }
}