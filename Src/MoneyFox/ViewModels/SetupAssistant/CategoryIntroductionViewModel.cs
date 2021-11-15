﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoneyFox.Extensions;
using Xamarin.Forms;

namespace MoneyFox.ViewModels.SetupAssistant
{
    public class CategoryIntroductionViewModel : ObservableObject
    {
        public RelayCommand GoToAddCategoryCommand
            => new RelayCommand(async () => await Shell.Current.GoToModalAsync(ViewModelLocator.AddCategoryRoute));

        public RelayCommand NextStepCommand
            => new RelayCommand(async () => await Shell.Current.GoToAsync(ViewModelLocator.SetupCompletionRoute));

        public RelayCommand BackCommand
            => new RelayCommand(async () => await Shell.Current.Navigation.PopAsync());
    }
}