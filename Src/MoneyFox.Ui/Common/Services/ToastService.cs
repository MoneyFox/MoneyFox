namespace MoneyFox.Ui.Common.Services;

using CommunityToolkit.Maui.Alerts;
using Core.Common.Interfaces;
using JetBrains.Annotations;

[UsedImplicitly]
public class ToastService : IToastService
{
    public async Task ShowToastAsync(string message, string title = "")
    {
        var toast = Toast.Make(message);
        await toast.Show();
    }
}
