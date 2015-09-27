using System;
using System.Threading.Tasks;
using MoneyManager.Foundation.Interfaces;

namespace MoneyManager.Ios
{
    public class DialogService : IDialogService
    {
        public Task ShowMessage(string title, string message)
        {
            throw new NotImplementedException();
        }

        public Task ShowConfirmMessage(string title, string message, Action positivAction,
            string positiveButtonText = null, string negativeButtonText = null, Action negativAction = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ShowConfirmMessage(string title, string message, string positiveButtonText = null,
            string negativeButtonText = null)
        {
            throw new NotImplementedException();
        }
    }
}