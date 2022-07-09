namespace MoneyFox.Common.Services
{

    using System.Threading.Tasks;
    using Core.Interfaces;
    using Extensions;
    using JetBrains.Annotations;
    using Xamarin.Forms;

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

}
