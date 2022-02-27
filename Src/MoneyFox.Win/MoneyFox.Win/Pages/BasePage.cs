namespace MoneyFox.Win.Pages;

using Microsoft.UI.Xaml.Controls;

public class BasePage : Page
{
    public virtual string Header => string.Empty;
    public virtual bool ShowHeader => true;
}