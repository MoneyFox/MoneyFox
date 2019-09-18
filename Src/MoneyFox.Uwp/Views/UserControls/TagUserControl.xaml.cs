using Windows.UI.Xaml.Controls;

namespace MoneyFox.Uwp.Views.UserControls
{
    public sealed partial class TagUserControl : UserControl
    {
        public TagUserControl()
        {
            InitializeComponent();

            WrapPanelContainer.Items.Add(new Button());
            WrapPanelContainer.Items.Add(new Button());
            WrapPanelContainer.Items.Add(new Button());
        }
    }
}
