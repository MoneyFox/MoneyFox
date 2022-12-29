namespace MoneyFox.Ui.Common.Services;

using Core.Interfaces;
using Extensions;
using JetBrains.Annotations;

[UsedImplicitly]
internal sealed class NavigationService : INavigationService
{
    public async Task NavigateToAsync<T>()
    {
        await Shell.Current.GoToAsync(typeof(T).Name);
    }

    public async Task OpenModalAsync<T>()
    {
        await Shell.Current.GoToModalAsync(typeof(T).Name);
    }

    public async Task GoBackFromModalAsync()
    {
        await Shell.Current.Navigation.PopModalAsync();
    }
}
