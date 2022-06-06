namespace MoneyFox.Services
{

    using System.Threading.Tasks;
    using Core.Interfaces;
    using Xamarin.Forms;

    internal sealed class NavigationService : INavigationService
    {
        public async Task NavigateTo<T>()
        {
            await Shell.Current.GoToAsync(typeof(T).Name);
        }

        public async Task GoBackFromModal()
        {
            await Shell.Current?.Navigation?.PopModalAsync();
        }
    }

}
