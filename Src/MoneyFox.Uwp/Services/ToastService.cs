using MoneyFox.Services;
using System.Threading.Tasks;

#nullable enable
namespace MoneyFox.Uwp.Services
{
    public class ToastService : IToastService
    {
        public Task ShowToastAsync(string message, string title = "") => Task.CompletedTask;
    }
}
