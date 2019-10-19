using MoneyFox.Presentation.ViewModels;

namespace MoneyFox.Presentation.Views
{
    public partial class SettingsPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.SettingsVm;

            SettingsList.ItemTapped += (sender, args) =>
            {
                SettingsList.SelectedItem = null;
                (BindingContext as SettingsViewModel)?.GoToSettingCommand.Execute(args.Item);
            };
        }
    }
}
