using MoneyFox.Application.Resources;

#nullable enable
namespace MoneyFox.Uwp.Views.Settings
{
    public sealed partial class SettingsHostView
    {
        public override string Header => Strings.SettingsTitle;

        public SettingsHostView()
        {
            InitializeComponent();
        }
    }
}