namespace MoneyFox.Ui.Common.Services;

using Extensions;
using JetBrains.Annotations;
using Microsoft.AppCenter.Analytics;
using MoneyFox.Ui;

[UsedImplicitly]
internal sealed class NavigationService : INavigationService
{
    public async Task NavigateToAsync<T>() where T : ContentPage
    {
        var pageName = typeof(T).Name;
        Analytics.TrackEvent($"Navigate to {pageName}");
        await Shell.Current.GoToAsync(pageName);
    }

    public async Task NavigateBackAsync()
    {
        Analytics.TrackEvent("Navigate back");
        await Shell.Current.GoToAsync("..");
    }

    public async Task NavigateBackAsync(string parameterName, string queryParameter)
    {
        Analytics.TrackEvent("Navigate back");
        await Shell.Current.GoToAsync($"..?{parameterName}={queryParameter}");
    }

    public async Task OpenModalAsync<T>()
    {
        var pageName = typeof(T).Name;
        Analytics.TrackEvent($"Navigate to {pageName}");
        await Shell.Current.GoToModalAsync(pageName);
    }

    public async Task GoBackFromModalAsync()
    {
        Analytics.TrackEvent("Navigate back from Modal");
        await Shell.Current.Navigation.PopModalAsync();
    }
}
