namespace MoneyFox.Services
{

    using System.Threading.Tasks;
    using Acr.UserDialogs;
    using Core.Common.Interfaces;
    using JetBrains.Annotations;

    [UsedImplicitly]
    public class ToastService : IToastService
    {
        private readonly IUserDialogs userDialogs;

        public ToastService(IUserDialogs userDialogs)
        {
            this.userDialogs = userDialogs;
        }

        public Task ShowToastAsync(string message, string title = "")
        {
            userDialogs.Toast(new ToastConfig(message));

            return Task.CompletedTask;
        }
    }

}
