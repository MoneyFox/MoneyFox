using MoneyFox.DataAccess.DataServices;

namespace MoneyFox.Windows.Business.Tiles
{
    public class LiveTileManager
    {
        private readonly IAccountService accountService;

        public LiveTileManager(IAccountService accountService)
        {
            this.accountService = accountService;
        }

    }
}
