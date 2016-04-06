using System;
using MoneyFox.Shared.ViewModels;

namespace MoneyManager.Core.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        public void ShowViewModelByType(Type viewModel)
        {
            ShowViewModel(viewModel);
        }
    }
}
