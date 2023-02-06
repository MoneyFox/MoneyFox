namespace MoneyFox.Ui.Common.Services;

using Core.Interfaces;
using Extensions;
using JetBrains.Annotations;
using Microsoft.AppCenter.Analytics;

[UsedImplicitly]
internal sealed class NavigationService : INavigationService
{
    public async Task NavigateToAsync<T>()
    {
        var pageName = typeof(T).Name;
        Analytics.TrackEvent($"Navigate to {pageName}");
        await Shell.Current.GoToAsync(pageName);
    }

    public async Task OpenModalAsync<T>()
    {
        var pageName = typeof(T).Name;
        Analytics.TrackEvent($"Navigate to {pageName}");
        await Shell.Current.GoToModalAsync(pageName);
    }

    public async Task GoBackFromModalAsync()
    {
        Analytics.TrackEvent($"Navigate back from Modal");
        await Shell.Current.Navigation.PopModalAsync();
    }
}
