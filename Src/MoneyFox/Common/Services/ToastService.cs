namespace MoneyFox.Common.Services
{

    using System.Threading.Tasks;
    using Core.Common.Interfaces;
    using JetBrains.Annotations;
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
