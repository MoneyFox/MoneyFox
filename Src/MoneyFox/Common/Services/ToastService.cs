namespace MoneyFox.Common.Services
{

    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using MoneyFox.Core.Common.Interfaces;
    using Xamarin.CommunityToolkit.Extensions;
    using Xamarin.Forms;

    [UsedImplicitly]
    public class ToastService : IToastService
    {
        public async Task ShowToastAsync(string message, string title = "")
        {
            await Application.Current.MainPage.DisplayToastAsync(message);
        }
    }

}
