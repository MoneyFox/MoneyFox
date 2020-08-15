using Plugin.Toast;

namespace MoneyFox.Services
{
    public class ToastService : IToastService
    {
        public void ShowToast(string text)
            => CrossToastPopUp.Current.ShowToastMessage(text);
    }
}
