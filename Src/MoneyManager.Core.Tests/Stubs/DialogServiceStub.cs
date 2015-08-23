using System;
using System.Threading.Tasks;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Core.Tests.Stubs
{
    public class DialogServiceStub : IDialogService
    {
        public async Task ShowMessage(string title, string message)
        {
            //Just do nothing
        }

        public async Task ShowConfirmMessage(string title, string message, Action positivAction,
            string positiveButtonText = null,
            string negativeButtonText = null, Action negativAction = null)
        {
            //Just do nothing
        }

        public async Task<bool> ShowConfirmMessage(string title, string message, string positiveButtonText = null,
            string negativeButtonText = null)
        {
            return true;
            //Just do nothing
        }
    }
}