﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Extensions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.ViewModels.SetupAssistant
{
    public class WelcomeViewModel : ObservableObject
    {
        private readonly ISettingsFacade settingsFacade;

        public WelcomeViewModel(ISettingsFacade settingsFacade)
        {
            this.settingsFacade = settingsFacade;
        }

        public async Task InitAsync()
        {
            if(settingsFacade.IsSetupCompleted)
            {
                await Shell.Current.GoToAsync(ViewModelLocator.DashboardRoute);
            }
        }

        public RelayCommand GoToAddAccountCommand
            => new RelayCommand(async () => await Shell.Current.GoToModalAsync(ViewModelLocator.AddAccountRoute));

        public RelayCommand NextStepCommand => new RelayCommand(
            async ()
                => await Shell.Current.GoToAsync(ViewModelLocator.CategoryIntroductionRoute));

        public RelayCommand SkipCommand => new RelayCommand(SkipSetup);

        private void SkipSetup()
        {
            settingsFacade.IsSetupCompleted = true;
            Xamarin.Forms.Application.Current.MainPage = new AppShell();
        }
    }
}