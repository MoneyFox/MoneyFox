using System.Threading.Tasks;
using MoneyFox.Foundation.Resources;
using UniversalRateReminder;

namespace MoneyFox.Uwp.Services
{
    internal static class RateDisplayService
    {
        internal static async Task ShowIfAppropriateAsync()
        {
            RatePopup.RateButtonText = Strings.YesLabel;
            RatePopup.CancelButtonText = Strings.NotNowLabel;
            RatePopup.Title = Strings.RateReminderTitle;
            RatePopup.Content = Strings.RateReminderText;

            await RatePopup.CheckRateReminderAsync();
        }
    }
}
