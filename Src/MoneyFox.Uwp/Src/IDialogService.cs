using System.Threading.Tasks;

namespace MoneyFox.Uwp.Src
{
    public interface IDialogService
    {
        Task HideLoadingDialogAsync();
        Task<bool> ShowConfirmMessageAsync(string title, string message, string positiveButtonText = null, string negativeButtonText = null);
        Task ShowLoadingDialogAsync(string message = null);
        Task ShowMessageAsync(string title, string message);
    }
}