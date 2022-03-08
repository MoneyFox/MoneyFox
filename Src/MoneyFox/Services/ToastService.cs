namespace MoneyFox.Services
{
    using Acr.UserDialogs;
    using Core.Common.Interfaces;
    using JetBrains.Annotations;
    using System.Threading.Tasks;

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