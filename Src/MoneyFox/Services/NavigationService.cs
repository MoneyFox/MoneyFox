namespace MoneyFox.Services
{
    using Core.Interfaces;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    internal sealed class NavigationService : INavigationService
    {
        public async Task NavigateTo<T>()
        {
            await Shell.Current.GoToAsync(typeof(T).Name);
        }
    }
}