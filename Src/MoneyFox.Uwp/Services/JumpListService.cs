using MoneyFox.Application.Common.Constants;
using MoneyFox.Application.Resources;
using NLog;
using System;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI.StartScreen;

#nullable enable
namespace MoneyFox.Uwp.Services
{
    internal static class JumpListService
    {
        private const string INCOME_ICON = "ms-appx:///Assets/IncomeTileIcon.png";

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static async Task InitializeAsync()
        {
            if(!ApiInformation.IsTypePresent("Windows.UI.StartScreen.JumpList"))
            {
                return;
            }

            try
            {
                JumpList jumpList = await JumpList.LoadCurrentAsync();
                jumpList.Items.Clear();
                jumpList.SystemGroupKind = JumpListSystemGroupKind.None;

                var listItemAddPayment = JumpListItem.CreateWithArguments(AppConstants.AddPaymentId, Strings.AddPaymentLabel);
                listItemAddPayment.Logo = new Uri(INCOME_ICON);
                jumpList.Items.Add(listItemAddPayment);

                await jumpList.SaveAsync();
            }
            catch(Exception ex)
            {
                logger.Warn(ex);
            }
        }
    }
}
