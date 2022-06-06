namespace MoneyFox.Core.Interfaces
{

    using System.Threading.Tasks;

    public interface INavigationService
    {
        Task NavigateTo<T>();

        Task GoBackFromModal();
    }

}
