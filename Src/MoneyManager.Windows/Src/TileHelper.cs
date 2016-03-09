using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using MoneyFox.Foundation.Constants;
using MoneyManager.Foundation;

namespace MoneyManager.Windows
{
    public class TileHelper : ViewModelBase
    {
        private readonly INavigationService navigationService;

        public TileHelper(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public bool DoNavigation(string tileId)
        {
            switch (tileId)
            {
                case Constants.ADD_INCOME_TILE_ID:
                    navigationService.NavigateTo(NavigationConstants.MODIFY_PAYMENT_VIEW, Enum.Parse(typeof(PaymentType), Constants.INCOME_TILE_ID));
                    return true;

                case Constants.ADD_EXPENSE_TILE_ID:
                    navigationService.NavigateTo(NavigationConstants.MODIFY_PAYMENT_VIEW, Enum.Parse(typeof(PaymentType), Constants.EXPENSE_TILE_ID));
                    return true;

                case Constants.ADD_TRANSFER_TILE_ID:
                    navigationService.NavigateTo(NavigationConstants.MODIFY_PAYMENT_VIEW, Enum.Parse(typeof(PaymentType), Constants.TRANSFER_TILE_ID));
                    return true;
                default:
                    return false;
            }
        }
    }
}