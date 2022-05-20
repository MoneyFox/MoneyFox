namespace MoneyFox.Services
{

    using System.Threading.Tasks;
    using Core.Interfaces;

    internal sealed class NavigationService : INavigationService
    {
        public async Task NavigateTo<T>()
        {
            await Shell.Current.GoToAsync(typeof(T).Name);
        }
    }

}
