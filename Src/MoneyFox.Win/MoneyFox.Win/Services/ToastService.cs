namespace MoneyFox.Win.Services;

using System.Threading.Tasks;
using Core.Common.Interfaces;

public class ToastService : IToastService
{
    public Task ShowToastAsync(string message, string title = "")
    {
        return Task.CompletedTask;
    }
}
