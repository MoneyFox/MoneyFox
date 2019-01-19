using Microsoft.Toolkit.Uwp.Notifications;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;

namespace MoneyFox.Windows.Business.Tiles
{
    public class LiveTileManager
    {
        private readonly IAccountService accountService;

        private ApplicationDataContainer Localsettings = ApplicationData.Current.LocalSettings;
        private const int numberOfPayments = 8;

        public LiveTileManager(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        public async Task UpdatePrimaryLiveTile()
        {
            if (await LiveTileHelper.IsPinned())
            {
                object b = Localsettings.Values["lastrun"];
                string lastrun = (string)b;
                string headertext = "";
                List<string> displaycontentmedium = new List<string>();
                List<string> displaycontentlarge = new List<string>();

                if (lastrun == "last")
                {
                    Localsettings.Values["lastrun"] = "next";
                    headertext = Strings.LiveTileUpcommingPayments;
                    displaycontentmedium = await GetPaymentsAsync(TileSizeOptions.Medium, PaymentInformation.Next);
                    displaycontentlarge = await GetPaymentsAsync(TileSizeOptions.Large, PaymentInformation.Next);
                }
                else
                {
                    Localsettings.Values["lastrun"] = "last";
                    headertext = Strings.LiveTilePastPayments;
                    displaycontentmedium = await GetPaymentsAsync(TileSizeOptions.Medium, PaymentInformation.Previous);
                    displaycontentlarge = await GetPaymentsAsync(TileSizeOptions.Large, PaymentInformation.Previous);
                }

                TileContent content = new TileContent()
                {
                    Visual = new TileVisual()
                    {
                        TileMedium = GetTileBinding(headertext, displaycontentmedium),
                        TileWide = GetTileBinding(headertext, displaycontentlarge),
                        TileLarge = GetTileBinding(headertext, displaycontentlarge),
                    }
                };

                TileNotification tn = new TileNotification(content.GetXml());
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tn);
            }
        }

        public async Task UpdateSecondaryLiveTiles()
        {
            var tiles = await SecondaryTile.FindAllForPackageAsync();
            List<string> displaycontent = new List<string>();
            displaycontent = await GetPaymentsAsync(TileSizeOptions.Large, PaymentInformation.Previous);

            if (tiles == null) return;

            foreach (SecondaryTile item in tiles)
            {
                Account acct = await accountService.GetById(int.Parse(item.TileId));
                TileContent content = new TileContent()
                {
                    Visual = new TileVisual()
                    {
                        TileSmall = new TileBinding()
                        {
                            Content = new TileBindingContentAdaptive()
                            {
                                Children =
                                {
                            new AdaptiveGroup()
                            {
                                Children =
                                {
                                    new AdaptiveSubgroup()
                                    {
                                        Children =
                                        {
                                            new AdaptiveText()
                                            {
                                                Text = acct.Data.Name,
                                                HintStyle = AdaptiveTextStyle.Caption
                                            },
                                            new AdaptiveText()
                                            {
                                            Text =  LiveTileHelper.TruncateNumber(acct.Data.CurrentBalance),
                                            HintStyle = AdaptiveTextStyle.Caption
                                            }
                                        }
                                    }
                                }
                            }
                            }
                            }
                        },
                        TileMedium = new TileBinding()
                        {
                            Content = new TileBindingContentAdaptive()
                            {
                                Children =
                                {
                            new AdaptiveGroup()
                            {
                                Children =
                                {
                                    new AdaptiveSubgroup()
                                    {
                                        Children =
                                        {
                                    new AdaptiveText()
                                            {
                                                Text = acct.Data.Name,
                                                HintStyle = AdaptiveTextStyle.Caption
                                            },
                                        new AdaptiveText()
                                            {
                                                Text = string.Format(Strings.LiveTileAccountBalance,acct.Data.CurrentBalance.ToString("C2")),
                                                HintStyle = AdaptiveTextStyle.CaptionSubtle
                                            },
                                            new AdaptiveText()
                                            {
                                            Text =  Strings.ExpenseLabel,
                                            HintStyle = AdaptiveTextStyle.Caption
                                            },
                                            new AdaptiveText()
                                            {
                                                Text = string.Format(Strings.LiveTileLastMonthsExpenses, DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(DateTime.Now.AddMonths(-1).Month), LiveTileHelper.TruncateNumber(GetMonthExpenses(DateTime.Now.AddMonths(-1).Month, DateTime.Now.Year,acct))),
                                                HintStyle = AdaptiveTextStyle.CaptionSubtle
                                            },
                                            new AdaptiveText()
                                            {
                                                Text = string.Format(Strings.LiveTileCurrentMonthsExpenses, DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(DateTime.Now.Month),LiveTileHelper.TruncateNumber(GetMonthExpenses(DateTime.Now.Month,DateTime.Now.Year,acct))),
                                                HintStyle = AdaptiveTextStyle.CaptionSubtle
                                            }
                                        }
                                    }
                                }
                            }
                        }
                            }
                        },
                        TileWide = new TileBinding()
                        {
                            Content = new TileBindingContentAdaptive()
                            {
                                Children =
                        {
                            new AdaptiveGroup()
                            {
                                Children =
                                {
                                    new AdaptiveSubgroup()
                                    {
                                        Children =
                                        {
                                            new AdaptiveText()
                                            {
                                                Text = acct.Data.Name,
                                                HintStyle = AdaptiveTextStyle.Caption
                                            },
                                        new AdaptiveText()
                                            {
                                                Text = string.Format(Strings.LiveTileAccountBalance,acct.Data.CurrentBalance.ToString("C2")),
                                                HintStyle = AdaptiveTextStyle.CaptionSubtle
                                            },
                                            new AdaptiveText()
                                            {
                                            Text =  Strings.ExpenseLabel,
                                            HintStyle = AdaptiveTextStyle.Caption
                                            },
                                            new AdaptiveText()
                                            {
                                                Text = string.Format(Strings.LiveTileLastMonthsExpenses, DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(DateTime.Now.AddMonths(-1).Month), GetMonthExpenses(DateTime.Now.AddMonths(-1).Month, DateTime.Now.Year,acct).ToString("C2")),
                                                HintStyle = AdaptiveTextStyle.CaptionSubtle
                                            },
                                            new AdaptiveText()
                                            {
                                                Text = string.Format(Strings.LiveTileCurrentMonthsExpenses, DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(DateTime.Now.Month),GetMonthExpenses(DateTime.Now.Month,DateTime.Now.Year,acct).ToString("C2")),
                                                HintStyle = AdaptiveTextStyle.CaptionSubtle
                                            }
                                        }
                                    }
                                }
                            }
                        }
                            }
                        },
                        TileLarge = new TileBinding()
                        {
                            Content = new TileBindingContentAdaptive()
                            {
                                Children =
                        {
                            new AdaptiveGroup()
                            {
                                Children =
                                {
                                    new AdaptiveSubgroup()
                                    {
                                        Children =
                                        {
                                            new AdaptiveText()
                                            {
                                                Text = acct.Data.Name,
                                                HintStyle = AdaptiveTextStyle.Caption
                                            },
                                        new AdaptiveText()
                                            {
                                                Text = string.Format(Strings.LiveTileAccountBalance, acct.Data.CurrentBalance.ToString("C2")),
                                                HintStyle = AdaptiveTextStyle.CaptionSubtle
                                            },
                                            new AdaptiveText()
                                            {
                                            Text =  Strings.ExpenseLabel,
                                            HintStyle = AdaptiveTextStyle.Caption
                                            },
                                            new AdaptiveText()
                                            {
                                                Text = string.Format(Strings.LiveTileLastMonthsExpenses, DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(DateTime.Now.AddMonths(-1).Month),GetMonthExpenses(DateTime.Now.AddMonths(-1).Month, DateTime.Now.Year,acct).ToString("C2")),
                                                HintStyle = AdaptiveTextStyle.CaptionSubtle
                                            },
                                            new AdaptiveText()
                                            {
                                                Text = string.Format(Strings.LiveTileCurrentMonthsExpenses, DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(DateTime.Now.Month),GetMonthExpenses(DateTime.Now.Month,DateTime.Now.Year,acct).ToString("C2")),
                                                HintStyle = AdaptiveTextStyle.CaptionSubtle
                                            },
                                            new AdaptiveText()
                                            {
                                                Text = Strings.LiveTilePastPayments,
                                                HintStyle = AdaptiveTextStyle.Caption
                                            },
                                            new AdaptiveText()
                                            {
                                                Text = displaycontent[0],
                                                HintStyle = AdaptiveTextStyle.CaptionSubtle
                                            },
                                                new AdaptiveText()
                                            {
                                                Text = displaycontent[1],
                                                HintStyle = AdaptiveTextStyle.CaptionSubtle
                                            },
                                            new AdaptiveText()
                                            {
                                                Text = displaycontent[2],
                                                HintStyle = AdaptiveTextStyle.CaptionSubtle
                                            },
                                            new AdaptiveText()
                                            {
                                                Text = displaycontent[3],
                                                HintStyle = AdaptiveTextStyle.CaptionSubtle
                                            }
                                            ,
                                            new AdaptiveText()
                                            {
                                                Text = displaycontent[4],
                                                HintStyle = AdaptiveTextStyle.CaptionSubtle
                                            },
                                            new AdaptiveText()
                                            {
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

        public double GetMonthExpenses(int month, int year, Account accountid)
        {
            double balance = 0.00;
            List<LiveTilesPaymentInfo> allpayment = new List<LiveTilesPaymentInfo>();
            List<PaymentEntity> payments = accountid.Data.ChargedPayments;

            foreach (PaymentEntity item in payments)
            {
                if (item.IsRecurring)
                {
                    if (item.Type != Foundation.PaymentType.Income)
                    {
                        allpayment.AddRange(GetReccurance(item));
                    }
                }
                else if (item.Type != Foundation.PaymentType.Income)
                {
                    CreateLiveTileInfos(item, allpayment, item.Date.Date);
                }
            }

            List<LiveTilesPaymentInfo> tiles = allpayment
                .Where(x => x.Mydate.Date.Month == month && x.Mydate.Date.Year == year)
                .ToList();

            foreach (LiveTilesPaymentInfo item in tiles)
            {
                balance += item.Myamount;
            }
            allpayment.Clear();
            return balance;
        }

        private TileBinding GetTileBinding(string headertext, List<string> displaycontentmedium)
        {
            return new TileBinding()
            {
                Content = new TileBindingContentAdaptive()
                {
                    Children = {
                        new AdaptiveGroup()
                        {
                            Children =
                            {
                                new AdaptiveSubgroup()
                                {
                                    Children =
                                    {
                                        new AdaptiveText()
                                        {
                                            Text = headertext,
                                            HintStyle = AdaptiveTextStyle.Caption
                                        },
                                        new AdaptiveText()
                                        {
                                            Text = displaycontentmedium[0],
                                            HintStyle = AdaptiveTextStyle.CaptionSubtle
                                        },
                                        new AdaptiveText()
                                        {
                                            Text = displaycontentmedium[1],
                                            HintStyle = AdaptiveTextStyle.CaptionSubtle
                                        },
                                            new AdaptiveText()
                                        {
                                            Text = displaycontentmedium[2],
                                            HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                        },
                                        new AdaptiveText()
                                        {
                                            Text = displaycontentmedium[3],
                                            HintStyle = AdaptiveTextStyle.CaptionSubtle
                                        },
                                        new AdaptiveText()
                                        {
                                            Text = displaycontentmedium[4],
                                            HintStyle = AdaptiveTextStyle.CaptionSubtle
                                        },new AdaptiveText()
                                        {
                                            Text = displaycontentmedium[5],
                                            HintStyle = AdaptiveTextStyle.CaptionSubtle
                                        },
                                        new AdaptiveText()
                                        {
                                            Text = displaycontentmedium[6],
                                            HintStyle=AdaptiveTextStyle.CaptionSubtle
                                        },
                                        new AdaptiveText()
                                        {
                                            Text = displaycontentmedium[7],
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

        private async Task<List<string>> GetPaymentsAsync(TileSizeOptions tilesize, PaymentInformation paymentInformation)
        {
            List<Account> acct = (await accountService.GetAllAccounts()).ToList();
            List<PaymentEntity> allpayments = new List<PaymentEntity>();
            List<LiveTilesPaymentInfo> allpayment = new List<LiveTilesPaymentInfo>();
            foreach (Account item in acct)
            {
                Account accts = await accountService.GetById(item.Data.Id);
                if (accts.Data.ChargedPayments != null)
                {
                    allpayments.AddRange(accts.Data.ChargedPayments);
                }
                if (accts.Data.TargetedPayments != null)
                {
                    allpayments.AddRange(accts.Data.TargetedPayments);
                }
            }

            foreach (PaymentEntity item in allpayments)
            {
                if (item.IsRecurring)
                {
                    allpayment.AddRange(GetReccurance(item));
                }
                else
                {
                    LiveTilesPaymentInfo tileinfo = new LiveTilesPaymentInfo();
                    tileinfo.Chargeaccountname = item.ChargedAccount.Name;
                    tileinfo.Myamount = item.Amount;
                    tileinfo.Mydate = item.Date.Date;
                    tileinfo.Type = item.Type;
                    allpayment.Add(tileinfo);
                }
            }

            List<LiveTilesPaymentInfo> payments;

            if (paymentInformation == PaymentInformation.Previous)
            {
                payments = allpayment.OrderByDescending(x => x.Mydate.Date)
                    .ThenBy(x => x.Mydate.Date <= DateTime.Today.Date)
                    .Take(numberOfPayments)
                    .ToList();
            }
            else
            {
                payments = allpayment.OrderBy(x => x.Mydate.Date)
                    .ThenBy(x => x.Mydate.Date >= DateTime.Today.Date)
                    .Take(numberOfPayments)
                    .ToList();
            }

            List<string> returnlist = payments.Select(x => LiveTileHelper.GetTileText(tilesize, x)).ToList();

            for (int i = returnlist.Count; i < (numberOfPayments - 1); i++)
            {
                returnlist.Add(string.Empty);
            }

            allpayments.Clear();
            return returnlist;
        }

        private List<LiveTilesPaymentInfo> GetReccurance(PaymentEntity payment)
        {
            List<LiveTilesPaymentInfo> allpayment = new List<LiveTilesPaymentInfo>();

            if (payment.RecurringPayment.IsEndless)
            {
                DateTime startDate = payment.RecurringPayment.StartDate;
                while (DateTime.Compare(DateTime.Now, startDate) <= 0)
                {
                    startDate = CreateLiveTileInfos(payment, allpayment, startDate);
                }
            }
            else
            {
                DateTime startDate = payment.RecurringPayment.StartDate;
                DateTime endDate = payment.RecurringPayment.EndDate.Value;
                while (DateTime.Compare(startDate, endDate) <= 0)
                {
                    startDate = CreateLiveTileInfos(payment, allpayment, startDate);
                }
            }
            return allpayment;
        }

        private DateTime CreateLiveTileInfos(PaymentEntity payment, List<LiveTilesPaymentInfo> allpayment, DateTime startDate)
        {
            var liveTilesPaymentInfo = new LiveTilesPaymentInfo
            {
                Mydate = startDate,
                Myamount = payment.RecurringPayment.Amount,
                Chargeaccountname = payment.RecurringPayment.ChargedAccount.Name,
                Type = payment.RecurringPayment.Type
            };
            allpayment.Add(liveTilesPaymentInfo);
            return LiveTileHelper.AddDateByRecurrence(payment, startDate);
        }
    }
}