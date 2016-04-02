using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using MoneyFox.Core.Constants;

namespace MoneyFox.Core.Helpers
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
                case Constants.Constants.ADD_INCOME_TILE_ID:
                    navigationService.NavigateTo(NavigationConstants.MODIFY_PAYMENT_VIEW,
                        Enum.Parse(typeof (PaymentType), Constants.Constants.INCOME_TILE_ID));
                    return true;

                case Constants.Constants.ADD_EXPENSE_TILE_ID:
                    navigationService.NavigateTo(NavigationConstants.MODIFY_PAYMENT_VIEW,
                        Enum.Parse(typeof (PaymentType), Constants.Constants.EXPENSE_TILE_ID));
                    return true;

                case Constants.Constants.ADD_TRANSFER_TILE_ID:
                    navigationService.NavigateTo(NavigationConstants.MODIFY_PAYMENT_VIEW,
                        Enum.Parse(typeof (PaymentType), Constants.Constants.TRANSFER_TILE_ID));
                    return true;
                default:
                    return false;
            }
        }
    }
}