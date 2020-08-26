using MoneyFox.Services;
using System.Threading.Tasks;

namespace MoneyFox.Uwp.Services
{
    public class ToastService : IToastService
    {
        public Task ShowToastAsync(string message, string title = "") { return Task.CompletedTask; }
    }
}
