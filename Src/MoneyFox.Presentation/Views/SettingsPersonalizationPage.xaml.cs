using MoneyFox.Application.Resources;
using MoneyFox.Presentation.ViewModels;
using System;
using System.Collections.Generic;

namespace MoneyFox.Presentation.Views
{
    public partial class SettingsPersonalizationPage
    {
        private SettingsPersonalizationViewModel ViewModel => BindingContext as SettingsPersonalizationViewModel;
        public SettingsPersonalizationPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.SettingsPersonalizationVm;

            ThemePicker.ItemsSource = new List<string> { Strings.LightLabel, Strings.DarkLabel };

            ThemePicker.SelectedIndex = ViewModel.ElementTheme == "Light" ? 0 : 1;
        }

        private void ThemePicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ViewModel.SwitchThemeCommand.Execute(ThemePicker.SelectedItem.ToString() == Strings.LightLabel ? "Light" : "Dark");
        }
    }
}