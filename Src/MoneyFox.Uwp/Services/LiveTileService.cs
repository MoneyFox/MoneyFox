using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Views;
using GenericServices;
using MoneyFox.Presentation;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.Uwp.Activation;
using NLog;

namespace MoneyFox.Uwp.Services
{
    internal partial class LiveTileService : ActivationHandler<LaunchActivatedEventArgs>
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public void UpdateTile(TileNotification notification)
        {
            try
            {
                TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
            } catch (Exception)
            {
                // TODO WTS: Updating LiveTile can fail in rare conditions, please handle exceptions as appropriate to your scenario.
            }
        }

        public async Task<bool> PinSecondaryTileAsync(SecondaryTile tile, bool allowDuplicity = false)
        {
            try
            {
                if (!await IsAlreadyPinnedAsync(tile) || allowDuplicity)
                {
                    return await tile.RequestCreateAsync();
                }

                return false;
            } catch (Exception)
            {
                // TODO WTS: Adding SecondaryTile can fail in rare conditions, please handle exceptions as appropriate to your scenario.
                return false;
            }
        }

        private async Task<bool> IsAlreadyPinnedAsync(SecondaryTile tile)
        {
            var secondaryTiles = await SecondaryTile.FindAllAsync();
            return secondaryTiles.Any(t => t.Arguments == tile.Arguments);
        }

        protected override async Task HandleInternalAsync(LaunchActivatedEventArgs args)
        {
            if(int.TryParse(args.TileId, out int accountId))
            {
                logger.Info("Open Payment List of Account with ID {accountId}", accountId);

                var navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
                var crudServices = ServiceLocator.Current.GetInstance<ICrudServicesAsync>();
                AccountViewModel acct = await crudServices.ReadSingleAsync<AccountViewModel>(accountId);

                navigationService.NavigateTo(ViewModelLocator.PaymentList, acct.Id);
            }
        }

        protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
        {
            return LaunchFromSecondaryTile(args);
        }

        private bool LaunchFromSecondaryTile(LaunchActivatedEventArgs args)
        {
            return int.TryParse(args.TileId, out int accountId);
        }
    }
}
