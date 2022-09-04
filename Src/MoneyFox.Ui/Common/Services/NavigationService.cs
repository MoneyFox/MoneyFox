namespace MoneyFox.Ui.Common.Services;

using Extensions;
using JetBrains.Annotations;
using MoneyFox.Core.Interfaces;

[UsedImplicitly]
internal sealed class NavigationService : INavigationService
{
    public async Task NavigateTo<T>()
    {
        await Shell.Current.GoToAsync(typeof(T).Name);
    }

    public async Task OpenModal<T>()
    {
        await Shell.Current.GoToModalAsync(typeof(T).Name);
    }

    public async Task GoBackFromModal()
    {
        await Shell.Current.Navigation.PopModalAsync();
    }
}
