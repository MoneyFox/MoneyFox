namespace MoneyFox.Win.Services;

using Core._Pending_.Common.Interfaces;
using System.Threading.Tasks;

public class ToastService : IToastService
{
    public Task ShowToastAsync(string message, string title = "") => Task.CompletedTask;
}