using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.UI.StartScreen;
using System;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Resources;
using MoneyFox.DataAccess.Entities;

namespace MoneyFox.Windows.Business.Tiles
{
    public static class LiveTileHelper
    {
        public static string TruncateNumber(double num)
        {
            if (num > 0 && num < 1000)
            {
                return num.ToString("#,0");
            }
            if (num > 1000 && num < 1000000)
            {
                double test = num / 1000;
                return test.ToString("#.0") + "K";
            }
            if (num > 1000000 && num < 1000000000)
            {
                double test = num / 1000000;
                return test.ToString("#.1") + "M";
            }
            if (num > 1000000000 & num < 1000000000000)
            {
                double test = num / 1000000000;
                return test.ToString("#.0") + "B";
            }
            return "Number out of Range";
        }

        public static async Task<bool> IsPinned()
        {
            AppListEntry entry = (await Package.Current.GetAppListEntriesAsync())[0];
            return await StartScreenManager.GetDefault().ContainsAppListEntryAsync(entry);
        }

        public static string GetTileText(TileSizeOptions tilesize, LiveTilesPaymentInfo liveTileItem)
        {
            if (liveTileItem.Type == PaymentType.Income)
            {
                switch (tilesize)
                {
                    case TileSizeOptions.Medium:
                        return liveTileItem.Chargeaccountname + " +" + TruncateNumber(liveTileItem.Amount);

                    case TileSizeOptions.Wide:
                    case TileSizeOptions.Large:
                        return string.Format(Strings.LiveTileWideandLargeIncomePastText, liveTileItem.Amount.ToString("C2"), liveTileItem.Chargeaccountname, liveTileItem.Date.Date);

                    default:
                        return string.Empty;
                }
            }
            else
            {
                switch (tilesize)
                {
                    case TileSizeOptions.Medium:
                        return liveTileItem.Chargeaccountname + " -" + TruncateNumber(liveTileItem.Amount);

                    case TileSizeOptions.Wide:
                    case TileSizeOptions.Large:
                        return string.Format(Strings.LiveTileWideandLargePaymentPastText, liveTileItem.Amount.ToString("C2"), liveTileItem.Chargeaccountname);

                    default:
                        return string.Empty;
                }
            }
        }

        public static DateTime AddDateByRecurrence(PaymentEntity payment, DateTime date)
        {
            if (payment.RecurringPayment == null) return date;

            switch (payment.RecurringPayment.Recurrence)
            {
                case PaymentRecurrence.Daily:
                    return date.AddDays(1);

                case PaymentRecurrence.DailyWithoutWeekend:
                    if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                    {
                        return date.AddDays(1);
                    }
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
