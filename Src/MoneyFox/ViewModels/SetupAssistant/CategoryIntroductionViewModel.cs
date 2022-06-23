namespace MoneyFox.ViewModels.SetupAssistant
{

    using Common.Extensions;
    using CommunityToolkit.Mvvm.Input;
    using Xamarin.Forms;

    internal sealed class CategoryIntroductionViewModel : BaseViewModel
    {
        public AsyncRelayCommand GoToAddCategoryCommand
            => new AsyncRelayCommand(async () => await Shell.Current.GoToModalAsync(ViewModelLocator.AddCategoryRoute));

        public AsyncRelayCommand NextStepCommand => new AsyncRelayCommand(async () => await Shell.Current.GoToAsync(ViewModelLocator.SetupCompletionRoute));

        public AsyncRelayCommand BackCommand => new AsyncRelayCommand(async () => await Shell.Current.Navigation.PopAsync());
    }

}
