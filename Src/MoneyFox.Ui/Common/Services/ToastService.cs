namespace MoneyFox.Ui.Common.Services;

using CommunityToolkit.Maui.Alerts;
using JetBrains.Annotations;
using MoneyFox.Core.Common.Interfaces;

[UsedImplicitly]
public class ToastService : IToastService
{
    public async Task ShowToastAsync(string message, string title = "")
    {
        var toast = Toast.Make(message);
        await toast.Show();
    }
}
