using System;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels
{
    /// <summary>
    ///     Represents the side menu
    /// </summary>
    public class MenuViewModel : BaseViewModel
    {
        // used for IOS Binding
        public MvxCommand ShowAccountListCommand
            => new MvxCommand(() => ShowViewModelByType(typeof(AccountListViewModel)));

        /// <summary>
        ///     Do a navigation based on the passed ViewModel type.
        /// </summary>
        /// <param name="viewModel">type of the viewmodel to navigate to.</param>
        public void ShowViewModelByType(Type viewModel)
        {
            ShowViewModel(viewModel);
        }
    }
}