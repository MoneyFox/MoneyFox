using System;

namespace MoneyFox.Shared.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        public void ShowViewModelByType(Type viewModel)
        {
            ShowViewModel(viewModel);
        }
    }
}