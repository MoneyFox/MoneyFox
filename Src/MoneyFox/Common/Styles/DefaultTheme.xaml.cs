using Xamarin.Forms;

[assembly: ExportFont("ProductSans-Regular.ttf", Alias = "Product")]
[assembly: ExportFont("RobotoMono-Regular.ttf", Alias = "Roboto")]
[assembly: ExportFont("norwester.otf", Alias = "Norwester")]

namespace MoneyFox.Common.Styles
{
    using Xamarin.Forms;

    public partial class DefaultTheme : ResourceDictionary
    {
        public DefaultTheme()
        {
            InitializeComponent();
        }
    }
}