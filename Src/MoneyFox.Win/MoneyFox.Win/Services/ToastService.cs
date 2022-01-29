using MoneyFox.Core._Pending_.Common.Interfaces;
using System.Threading.Tasks;

#nullable enable
namespace MoneyFox.Win.Services
{
    public class ToastService : IToastService
    {
        public Task ShowToastAsync(string message, string title = "") => Task.CompletedTask;
    }
}