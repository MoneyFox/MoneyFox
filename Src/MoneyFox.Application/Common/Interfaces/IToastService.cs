using System.Threading.Tasks;

namespace MoneyFox.Services
{
    public interface IToastService
    {
        Task ShowToastAsync(string title, string text);
    }
}
