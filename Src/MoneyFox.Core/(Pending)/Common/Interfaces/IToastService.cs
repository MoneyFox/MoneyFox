using System.Threading.Tasks;

namespace MoneyFox.Core._Pending_.Common.Interfaces
{
    public interface IToastService
    {
        Task ShowToastAsync(string message, string title = "");
    }
}