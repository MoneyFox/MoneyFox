namespace MoneyFox.Core.Interfaces
{
    using System;
    using System.Threading.Tasks;

    public interface INavigationService
    {
        Task NavigateTo<T>();
    }
}