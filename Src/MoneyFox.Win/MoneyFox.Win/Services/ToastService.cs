namespace MoneyFox.Win.Services;

using Core.Common.Interfaces;
using System.Threading.Tasks;

public class ToastService : IToastService
{
    public Task ShowToastAsync(string message, string title = "") => Task.CompletedTask;
}