using Windows.UI.Xaml.Controls;

namespace MoneyFox.Uwp.Views.Controls
{
    public sealed partial class TagControl : UserControl
    {
        public TagControl()
        {
            InitializeComponent();

            WrapPanelContainer.Items.Add(new Button());
            WrapPanelContainer.Items.Add(new Button());
            WrapPanelContainer.Items.Add(new Button());
        }
    }
}
