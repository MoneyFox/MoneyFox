using System;
using System.Threading.Tasks;
using Windows.UI.StartScreen;
using MoneyFox.Business.Parameters;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MvvmCross.Navigation;

namespace MoneyFox.Windows
{
    public class TileManager : ITileManager
    {
        private IMvxNavigationService navigationService;

        public TileManager(IMvxNavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public bool Exists(TyleType type)
        {
            return SecondaryTile.Exists(GetStringId(type));
        }

        public async void CreateTile(TyleType type)
        {
            var informations = GetTileInformation(type);

            var secondaryTile = new SecondaryTile(
                            informations.Item1,
                            informations.Item2,
                            informations.Item1,
                            informations.Item3,
                            TileSize.Default);

            secondaryTile.VisualElements.ShowNameOnSquare150x150Logo = true;
            await secondaryTile.RequestCreateAsync();
        }

        public async void RemoveTile(TyleType type)
        {
            await new SecondaryTile(GetStringId(type)).RequestDeleteAsync();
        }

        public async Task<bool> DoNavigation(string tileId)
        {
            switch (tileId)
            {
                case Constants.ADD_INCOME_TILE_ID:
                    await navigationService.Navigate<ModifyPaymentViewModel, ModifyPaymentParameter>(new ModifyPaymentParameter(PaymentType.Income));
                    return true;

                case Constants.ADD_EXPENSE_TILE_ID:
                    await navigationService.Navigate<ModifyPaymentViewModel, ModifyPaymentParameter>(new ModifyPaymentParameter(PaymentType.Expense));
                    return true;

                case Constants.ADD_TRANSFER_TILE_ID:
                    await navigationService.Navigate<ModifyPaymentViewModel, ModifyPaymentParameter>(new ModifyPaymentParameter(PaymentType.Transfer));
                    return true;
                default:
                    return false;
            }
        }

        private string GetStringId(TyleType type)
        {
            switch (type)
            {
                case TyleType.Income:
                    return Constants.ADD_INCOME_TILE_ID;
                case TyleType.Expense:
                    return Constants.ADD_EXPENSE_TILE_ID;
                case TyleType.Transfer:
                    return Constants.ADD_TRANSFER_TILE_ID;
                default:
                    return string.Empty;
            }
        }

        private Tuple<string, string, Uri> GetTileInformation(TyleType type)
        {
            switch (type)
            {
                case TyleType.Income:
                    return new Tuple<string, string, Uri>(Constants.ADD_INCOME_TILE_ID, Strings.AddIncomeLabel, new Uri("ms-appx:///Assets/IncomeTileIcon.png", UriKind.Absolute));
                case TyleType.Expense:
                    return new Tuple<string, string, Uri>(Constants.ADD_EXPENSE_TILE_ID, Strings.AddExpenseLabel, new Uri("ms-appx:///Assets/SpendingTileIcon.png", UriKind.Absolute));
                case TyleType.Transfer:
                    return new Tuple<string, string, Uri>(Constants.ADD_TRANSFER_TILE_ID, Strings.AddTransferLabel, new Uri("ms-appx:///Assets/TransferTileIcon.png", UriKind.Absolute));
                default:
                    return null;
            }
        }


    }
}