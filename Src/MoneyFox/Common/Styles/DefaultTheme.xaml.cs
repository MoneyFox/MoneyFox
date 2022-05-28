[assembly: ExportFont("ProductSans-Regular.ttf", Alias = "Product")]
[assembly: ExportFont("RobotoMono-Regular.ttf", Alias = "Roboto")]
[assembly: ExportFont("norwester.otf", Alias = "Norwester")]
[assembly: ExportFont("MaterialIconsRound-Regular.otf", Alias = "MaterialIconsRound")]

namespace MoneyFox.Common.Styles
{
    public partial class DefaultTheme : ResourceDictionary
    {
        public DefaultTheme()
        {
            InitializeComponent();
        }
    }

}
