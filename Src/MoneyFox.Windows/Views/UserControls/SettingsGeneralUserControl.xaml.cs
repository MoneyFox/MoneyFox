
namespace MoneyFox.Windows.Views.UserControls
{
    public sealed partial class SettingsGeneralUserControl
    {
        public global::Windows.Storage.ApplicationDataContainer localSettings = global::Windows.Storage.ApplicationData.Current.LocalSettings;
        public global::Windows.Storage.StorageFolder localFolder = global::Windows.Storage.ApplicationData.Current.LocalFolder;

        public SettingsGeneralUserControl()
        {
            InitializeComponent();
        }

        private void default_language_box_SelectionChanged(object sender, global::Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
        {
            localSettings.Values["LangBool"] = true;
            
            if (default_language_box.SelectedIndex == 0)
            {
                localSettings.Values["ChosenLang"] = "de";
            }
            if (default_language_box.SelectedIndex == 1)
            {
                localSettings.Values["ChosenLang"] = "en";
            }
            if (default_language_box.SelectedIndex == 2)
            {
                localSettings.Values["ChosenLang"] = "ru";
            }
            if (default_language_box.SelectedIndex == 3)
            {
                localSettings.Values["ChosenLang"] = "es";
            }
            if (default_language_box.SelectedIndex == 4)
            {
                localSettings.Values["ChosenLang"] = "pt";
            }
            if (default_language_box.SelectedIndex == 5)
            {
                localSettings.Values["ChosenLang"] = "cn";
            }
        }
    }
}