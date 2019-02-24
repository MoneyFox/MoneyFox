using System;
using System.Globalization;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.StartScreen;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.ViewModels;
using MoneyFox.Windows.Business;
using MoneyFox.Windows.Business.Tiles;

namespace MoneyFox.Uwp.Business.Tiles
{
    public static class LiveTileHelper
    {
        public static string TruncateNumber(double num)
        {
            if (num > 0 && num < 1000) return num.ToString("#,0", CultureInfo.InvariantCulture);
            if (num > 1000 && num < 1000000)
            {
                var test = num / 1000;
                return test.ToString("#.0", CultureInfo.InvariantCulture) + "K";
            }

            if (num > 1000000 && num < 1000000000)
            {
                var test = num / 1000000;
                return test.ToString("#.1", CultureInfo.InvariantCulture) + "M";
            }

            if ((num > 1000000000) && (num < 1000000000000))
            {
                var test = num / 1000000000;
                return test.ToString("#.0", CultureInfo.InvariantCulture) + "B";
            }

            return "Number out of Range";
        }

        public static async Task<bool> IsPinned()
        {
            var entry = (await Package.Current.GetAppListEntriesAsync())[0];
            return await StartScreenManager.GetDefault().ContainsAppListEntryAsync(entry);
        }

        public static string GetTileText(TileSizeOptions tileSize, LiveTilesPaymentInfo liveTileItem)
        {
            if (liveTileItem.Type == PaymentType.Income)
                switch (tileSize)
                {
                    case TileSizeOptions.Medium:
                        return liveTileItem.Chargeaccountname + " +" + TruncateNumber(liveTileItem.Amount);

                    case TileSizeOptions.Wide:
                    case TileSizeOptions.Large:
                        return string.Format(CultureInfo.InvariantCulture, Strings.LiveTileWideandLargeIncomePastText,
                            liveTileItem.Amount.ToString("C2", CultureInfo.InvariantCulture),
                            liveTileItem.Chargeaccountname, liveTileItem.Date.Date);

                    default:
                        return string.Empty;
                }
            switch (tileSize)
            {
                case TileSizeOptions.Medium:
                    return liveTileItem.Chargeaccountname + " -" + TruncateNumber(liveTileItem.Amount);

                case TileSizeOptions.Wide:
                case TileSizeOptions.Large:
                    return string.Format(CultureInfo.InvariantCulture, Strings.LiveTileWideandLargePaymentPastText,
                        liveTileItem.Amount.ToString("C2", CultureInfo.InvariantCulture),
                        liveTileItem.Chargeaccountname);

                default:
                    return string.Empty;
            }
        }

        public static DateTime AddDateByRecurrence(PaymentViewModel payment, DateTime date)
        {
            if (payment.RecurringPayment == null) return date;

            switch (payment.RecurringPayment.Recurrence)
            {
                case PaymentRecurrence.Daily:
                    return date.AddDays(1);

                case PaymentRecurrence.DailyWithoutWeekend:
                    if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                        return date.AddDays(1);
                    return date;

                case PaymentRecurrence.Weekly:
                    return date.AddDays(7);

                case PaymentRecurrence.Biweekly:
                    return date.AddDays(14);

                case PaymentRecurrence.Monthly:
                    return date.AddMonths(1);

                case PaymentRecurrence.Bimonthly:
                    return date.AddMonths(2);

                case PaymentRecurrence.Quarterly:
                    return date.AddMonths(3);

                case PaymentRecurrence.Yearly:
                    return date.AddMonths(12);

                case PaymentRecurrence.Biannually:
                    return date.AddMonths(24);

                default:
                    return date;
            }
        }
    }
}