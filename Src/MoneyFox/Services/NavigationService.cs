namespace MoneyFox.Services
{
    using Core.Interfaces;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    internal sealed class NavigationService : INavigationService
    {
        public async Task NavigateTo<T>(T pageType) => await Shell.Current.GoToAsync(pageType.ToString());
    }
}