namespace MoneyFox.Core.Interfaces;

using System.Threading.Tasks;

public interface INavigationService
{
    Task NavigateToAsync<T>();

    Task NavigateBackAsync();

    Task OpenModalAsync<T>();

    Task GoBackFromModalAsync();
}
