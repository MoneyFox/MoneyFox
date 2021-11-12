using System.Threading.Tasks;

namespace MoneyFox.Application.Common.Interfaces
{
    public interface IToastService
    {
        Task ShowToastAsync(string message, string title = "");
    }
}
