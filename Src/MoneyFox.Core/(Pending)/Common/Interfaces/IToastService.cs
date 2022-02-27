namespace MoneyFox.Core._Pending_.Common.Interfaces
{
    using System.Threading.Tasks;

    public interface IToastService
    {
        Task ShowToastAsync(string message, string title = "");
    }
}