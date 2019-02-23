using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Uwp.Notifications;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.ViewModels;
using MoneyFox.Windows.Business;
using MoneyFox.Windows.Business.Tiles;

namespace MoneyFox.Uwp.Business.Tiles {
    public class LiveTileManager {
        private const int NUMBER_OF_PAYMENTS = 8;
        private readonly ICrudServicesAsync crudService;

        private readonly ApplicationDataContainer localsettings = ApplicationData.Current.LocalSettings;

        public LiveTileManager(ICrudServicesAsync crudService) {
            this.crudService = crudService;
        }

        public async Task UpdatePrimaryLiveTile() {
            if (await LiveTileHelper.IsPinned().ConfigureAwait(true)) {
                object b = localsettings.Values["lastrun"];
                string lastrun = (string) b;
                string headertext = "";
                List<string> displaycontentmedium = new List<string>();
                List<string> displaycontentlarge = new List<string>();

                if (lastrun == "last") {
                    localsettings.Values["lastrun"] = "next";
                    headertext = Strings.LiveTileUpcommingPayments;
                    displaycontentmedium = await GetPaymentsAsync(TileSizeOptions.Medium, PaymentInformation.Next).ConfigureAwait(true);
                    displaycontentlarge = await GetPaymentsAsync(TileSizeOptions.Large, PaymentInformation.Next).ConfigureAwait(true);
                }
                else {
                    localsettings.Values["lastrun"] = "last";
                    headertext = Strings.LiveTilePastPayments;
                    displaycontentmedium = await GetPaymentsAsync(TileSizeOptions.Medium, PaymentInformation.Previous).ConfigureAwait(true);
                    displaycontentlarge = await GetPaymentsAsync(TileSizeOptions.Large, PaymentInformation.Previous).ConfigureAwait(true);
                }

                TileContent content = new TileContent {
                    Visual = new TileVisual {
                        TileMedium = GetTileBinding(headertext, displaycontentmedium),
                        TileWide = GetTileBinding(headertext, displaycontentlarge),
                        TileLarge = GetTileBinding(headertext, displaycontentlarge)
                    }
                };

                TileNotification tn = new TileNotification(content.GetXml());
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tn);
            }
        }

        public async Task UpdateSecondaryLiveTiles() {
            var tiles = await SecondaryTile.FindAllForPackageAsync();
            List<string> displaycontent = new List<string>();
            displaycontent = await GetPaymentsAsync(TileSizeOptions.Large, PaymentInformation.Previous)
                .ConfigureAwait(true);

            if (tiles == null) return;

            foreach (SecondaryTile item in tiles) {
                AccountViewModel acct = await crudService.ReadSingleAsync<AccountViewModel>(item.TileId)
                                                         .ConfigureAwait(true);
                TileContent content = new TileContent {
                    Visual = new TileVisual {
                        TileSmall = new TileBinding {
                            Content = new TileBindingContentAdaptive {
                                Children = {
                                    new AdaptiveGroup {
                                        Children = {
                                            new AdaptiveSubgroup {
                                                Children = {
                                                    new AdaptiveText {
                                                        Text = acct.Name,
                                                        HintStyle = AdaptiveTextStyle.Caption
                                                    },
                                                    new AdaptiveText {
                                                        Text = LiveTileHelper.TruncateNumber(acct.CurrentBalance),
                                                        HintStyle = AdaptiveTextStyle.Caption
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        TileMedium = new TileBinding {
                            Content = new TileBindingContentAdaptive {
                                Children = {
                                    new AdaptiveGroup {
                                        Children = {
                                            new AdaptiveSubgroup {
                                                Children = {
                                                    new AdaptiveText {
                                                        Text = acct.Name,
                                                        HintStyle = AdaptiveTextStyle.Caption
                                                    },
                                                    new AdaptiveText {
                                                        Text = string.Format(CultureInfo.InvariantCulture,
                                                                             Strings.LiveTileAccountBalance,
                                                                             acct.CurrentBalance.ToString("C2", CultureInfo.InvariantCulture)),
                                                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                                                    },
                                                    new AdaptiveText {
                                                        Text = Strings.ExpenseLabel,
                                                        HintStyle = AdaptiveTextStyle.Caption
                                                    },
                                                    new AdaptiveText {
                                                        Text = string.Format(CultureInfo.InvariantCulture,
                                                                             Strings.LiveTileLastMonthsExpenses,
                                                                             DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(
                                                                                 DateTime.Now.AddMonths(-1).Month),
                                                                             LiveTileHelper.TruncateNumber(
                                                                                 GetMonthExpenses(
                                                                                     DateTime.Now.AddMonths(-1).Month, DateTime.Now.Year,
                                                                                     acct))),
                                                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                                                    },
                                                    new AdaptiveText {
                                                        Text = string.Format(CultureInfo.InvariantCulture,
                                                                             Strings.LiveTileCurrentMonthsExpenses,
                                                                             DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(
                                                                                 DateTime.Now.Month),
                                                                             LiveTileHelper.TruncateNumber(
                                                                                 GetMonthExpenses(
                                                                                     DateTime.Now.Month, DateTime.Now.Year, acct))),
                                                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        TileWide = new TileBinding {
                            Content = new TileBindingContentAdaptive {
                                Children = {
                                    new AdaptiveGroup {
                                        Children = {
                                            new AdaptiveSubgroup {
                                                Children = {
                                                    new AdaptiveText {
                                                        Text = acct.Name,
                                                        HintStyle = AdaptiveTextStyle.Caption
                                                    },
                                                    new AdaptiveText {
                                                        Text = string.Format(CultureInfo.InvariantCulture,
                                                                             Strings.LiveTileAccountBalance,
                                                                             acct.CurrentBalance.ToString("C2", CultureInfo.InvariantCulture)),
                                                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                                                    },
                                                    new AdaptiveText {
                                                        Text = Strings.ExpenseLabel,
                                                        HintStyle = AdaptiveTextStyle.Caption
                                                    },
                                                    new AdaptiveText {
                                                        Text = string.Format(CultureInfo.InvariantCulture,
                                                                             Strings.LiveTileLastMonthsExpenses,
                                                                             DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(
                                                                                 DateTime.Now.AddMonths(-1).Month),
                                                                             GetMonthExpenses(
                                                                                 DateTime.Now.AddMonths(-1).Month, DateTime.Now.Year,
                                                                                 acct).ToString("C2", CultureInfo.InvariantCulture)),
                                                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                                                    },
                                                    new AdaptiveText {
                                                        Text = string.Format(CultureInfo.InvariantCulture,
                                                                             Strings.LiveTileCurrentMonthsExpenses,
                                                                             DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(
                                                                                 DateTime.Now.Month),
                                                                             GetMonthExpenses(
                                                                                     DateTime.Now.Month, DateTime.Now.Year, acct)
                                                                                 .ToString("C2", CultureInfo.InvariantCulture)),
                                                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        TileLarge = new TileBinding {
                            Content = new TileBindingContentAdaptive {
                                Children = {
                                    new AdaptiveGroup {
                                        Children = {
                                            new AdaptiveSubgroup {
                                                Children = {
                                                    new AdaptiveText {
                                                        Text = acct.Name,
                                                        HintStyle = AdaptiveTextStyle.Caption
                                                    },
                                                    new AdaptiveText {
                                                        Text = string.Format(CultureInfo.InvariantCulture,
                                                                             Strings.LiveTileAccountBalance,
                                                                             acct.CurrentBalance.ToString("C2", CultureInfo.InvariantCulture)),
                                                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                                                    },
                                                    new AdaptiveText {
                                                        Text = Strings.ExpenseLabel,
                                                        HintStyle = AdaptiveTextStyle.Caption
                                                    },
                                                    new AdaptiveText {
                                                        Text = string.Format(CultureInfo.InvariantCulture,
                                                                             Strings.LiveTileLastMonthsExpenses,
                                                                             DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(
                                                                                 DateTime.Now.AddMonths(-1).Month),
                                                                             GetMonthExpenses(
                                                                                 DateTime.Now.AddMonths(-1).Month, DateTime.Now.Year,
                                                                                 acct).ToString("C2", CultureInfo.InvariantCulture)),
                                                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                                                    },
                                                    new AdaptiveText {
                                                        Text = string.Format(CultureInfo.InvariantCulture,
                                                                             Strings.LiveTileCurrentMonthsExpenses,
                                                                             DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(
                                                                                 DateTime.Now.Month),
                                                                             GetMonthExpenses(
                                                                                     DateTime.Now.Month, DateTime.Now.Year, acct)
                                                                                 .ToString("C2", CultureInfo.InvariantCulture)),
                                                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                                                    },
                                                    new AdaptiveText {
                                                        Text = Strings.LiveTilePastPayments,
                                                        HintStyle = AdaptiveTextStyle.Caption
                                                    },
                                                    new AdaptiveText {
                                                        Text = displaycontent[0],
                                                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                                                    },
                                                    new AdaptiveText {
                                                        Text = displaycontent[1],
                                                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                                                    },
                                                    new AdaptiveText {
                                                        Text = displaycontent[2],
                                                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                                                    },
                                                    new AdaptiveText {
                                                        Text = displaycontent[3],
                                                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                                                    },
                                                    new AdaptiveText {
                                                        Text = displaycontent[4],
                                                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                                                    },
                                                    new AdaptiveText {
                                                        Text = displaycontent[5],
                                                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                };

                TileNotification tn = new TileNotification(content.GetXml());
                TileUpdateManager.CreateTileUpdaterForSecondaryTile(item.TileId).Update(tn);
            }
        }

        public double GetMonthExpenses(int month, int year, AccountViewModel account) {
            double balance = 0.00;
            List<LiveTilesPaymentInfo> allpayment = new List<LiveTilesPaymentInfo>();
            List<PaymentViewModel> payments = crudService.ReadManyNoTracked<PaymentViewModel>()
                                                         .Where(x => x.ChargedAccountId == account.Id)
                                                         .ToList();

            foreach (PaymentViewModel item in payments) {
                if (item.IsRecurring) {
                    if (item.Type != PaymentType.Income) {
                        allpayment.AddRange(GetRecurrence(item));
                    }
                }
                else if (item.Type != PaymentType.Income) {
                    CreateLiveTileInfos(item, allpayment, item.Date.Date);
                }
            }

            List<LiveTilesPaymentInfo> tiles = allpayment
                .Where(x => x.Date.Date.Month == month && x.Date.Date.Year == year)
                .ToList();

            foreach (LiveTilesPaymentInfo item in tiles) {
                balance += item.Amount;
            }

            allpayment.Clear();
            return balance;
        }

        private TileBinding GetTileBinding(string headerText, List<string> displayContentMedium) {
            return new TileBinding {
                Content = new TileBindingContentAdaptive {
                    Children = {
                        new AdaptiveGroup {
                            Children = {
                                new AdaptiveSubgroup {
                                    Children = {
                                        new AdaptiveText {
                                            Text = headerText,
                                            HintStyle = AdaptiveTextStyle.Caption
                                        },
                                        new AdaptiveText {
                                            Text = displayContentMedium[0],
                                            HintStyle = AdaptiveTextStyle.CaptionSubtle
                                        },
                                        new AdaptiveText {
                                            Text = displayContentMedium[1],
                                            HintStyle = AdaptiveTextStyle.CaptionSubtle
                                        },
                                        new AdaptiveText {
                                            Text = displayContentMedium[2],
                                            HintStyle = AdaptiveTextStyle.CaptionSubtle
                                        },
                                        new AdaptiveText {
                                            Text = displayContentMedium[3],
                                            HintStyle = AdaptiveTextStyle.CaptionSubtle
                                        },
                                        new AdaptiveText {
                                            Text = displayContentMedium[4],
                                            HintStyle = AdaptiveTextStyle.CaptionSubtle
                                        },
                                        new AdaptiveText {
                                            Text = displayContentMedium[5],
                                            HintStyle = AdaptiveTextStyle.CaptionSubtle
                                        },
                                        new AdaptiveText {
                                            Text = displayContentMedium[6],
                                            HintStyle = AdaptiveTextStyle.CaptionSubtle
                                        },
                                        new AdaptiveText {
                                            Text = displayContentMedium[7],
                                            HintStyle = AdaptiveTextStyle.CaptionSubtle
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        private async Task<List<string>> GetPaymentsAsync(TileSizeOptions tileSize,
                                                          PaymentInformation paymentInformation) {
            List<AccountViewModel> acct = await crudService.ReadManyNoTracked<AccountViewModel>()
                                                           .ToListAsync()
                                                           .ConfigureAwait(true);
            List<PaymentViewModel> allpayments = new List<PaymentViewModel>();
            List<LiveTilesPaymentInfo> allpayment = new List<LiveTilesPaymentInfo>();

            foreach (AccountViewModel item in acct) {
                allpayments.AddRange(crudService.ReadManyNoTracked<PaymentViewModel>()
                                                .Where(x => x.ChargedAccountId == item.Id)
                                                .ToList());

                allpayments.AddRange(crudService.ReadManyNoTracked<PaymentViewModel>()
                                                .Where(x => x.TargetAccountId == item.Id)
                                                .ToList());
            }

            foreach (PaymentViewModel item in allpayments) {
                if (item.IsRecurring) {
                    allpayment.AddRange(GetRecurrence(item));
                }
                else {
                    var tileinfo = new LiveTilesPaymentInfo();
                    tileinfo.Chargeaccountname = item.ChargedAccount.Name;
                    tileinfo.Amount = item.Amount;
                    tileinfo.Date = item.Date.Date;
                    tileinfo.Type = item.Type;
                    allpayment.Add(tileinfo);
                }
            }

            List<LiveTilesPaymentInfo> payments;

            if (paymentInformation == PaymentInformation.Previous) {
                payments = allpayment.OrderByDescending(x => x.Date.Date)
                                     .ThenBy(x => x.Date.Date <= DateTime.Today.Date)
                                     .Take(NUMBER_OF_PAYMENTS)
                                     .ToList();
            }
            else {
                payments = allpayment.OrderBy(x => x.Date.Date)
                                     .ThenBy(x => x.Date.Date >= DateTime.Today.Date)
                                     .Take(NUMBER_OF_PAYMENTS)
                                     .ToList();
            }

            List<string> returnList = payments.Select(x => LiveTileHelper.GetTileText(tileSize, x)).ToList();

            for (int i = returnList.Count; i < NUMBER_OF_PAYMENTS - 1; i++) {
                returnList.Add(string.Empty);
            }

            allpayments.Clear();
            return returnList;
        }

        private List<LiveTilesPaymentInfo> GetRecurrence(PaymentViewModel payment) {
            List<LiveTilesPaymentInfo> allPayment = new List<LiveTilesPaymentInfo>();

            if (payment.RecurringPayment.IsEndless) {
                DateTime startDate = payment.RecurringPayment.StartDate;
                while (DateTime.Compare(DateTime.Now, startDate) <= 0) {
                    startDate = CreateLiveTileInfos(payment, allPayment, startDate);
                }
            }
            else {
                DateTime startDate = payment.RecurringPayment.StartDate;
                DateTime endDate = payment.RecurringPayment.EndDate.Value;
                while (DateTime.Compare(startDate, endDate) <= 0) {
                    startDate = CreateLiveTileInfos(payment, allPayment, startDate);
                }
            }

            return allPayment;
        }

        private DateTime CreateLiveTileInfos(PaymentViewModel payment, List<LiveTilesPaymentInfo> allPayment,
                                             DateTime startDate) {
            var liveTilesPaymentInfo = new LiveTilesPaymentInfo {
                Date = startDate,
                Amount = payment.RecurringPayment?.Amount ?? payment.Amount,
                Chargeaccountname = payment.RecurringPayment == null
                    ? payment.ChargedAccount.Name
                    : payment.RecurringPayment?.ChargedAccount.Name,
                Type = payment.RecurringPayment?.Type ?? payment.Type
            };
            allPayment.Add(liveTilesPaymentInfo);
            return LiveTileHelper.AddDateByRecurrence(payment, startDate);
        }
    }
}