using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFramework.DbContextScope;
using Microsoft.Toolkit.Uwp.Notifications;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Resources;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;

namespace MoneyFox.Windows.Business
{
	public static class CommonFunctions
	{

		private static AccountService accountService = new AccountService(new AmbientDbContextLocator(), new DbContextScopeFactory());
		private static PaymentService paymentService = new PaymentService(new AmbientDbContextLocator(), new DbContextScopeFactory());
		private static Dictionary<Foundation.PaymentRecurrence, Func<CommonFunctions.iReccurance>> strategy = new Dictionary<Foundation.PaymentRecurrence, Func<CommonFunctions.iReccurance>>();
		private static ApplicationDataContainer Localsettings = ApplicationData.Current.LocalSettings;
		private static List<int> reccuringPaymentIds = new List<int>();
		

		public static string TruncateNumber(double num)
		{
			if (num>0 && num<1000)
			{
				return num.ToString("#,0");
			}
			if (num>1000 && num<1000000)
			{
				double test = num / 1000;
				return test.ToString("#.0")+"K";
			}
			if (num>1000000 && num<1000000000)
			{
				double test = num / 1000000;
				return test.ToString("#.1") + "M";
			}
			if (num>1000000000 & num<1000000000000)
			{
				double test = num / 1000000000;
				return test.ToString("#.0") + "B";
			}
			return "Number out of Range";

		}
		
		public static string GetResourceKey(string keytofind)
		{
			System.Resources.ResourceManager keyValuePairs = Strings.ResourceManager;
			return keyValuePairs.GetString(keytofind);
		}
	
		public static double GetMonthExpenses(int month, int year, Account accountid)
		{
			reccuringPaymentIds.Clear();
			double balance = 0.00;
			List<LiveTilesPaymentInfo> allpayment = new List<LiveTilesPaymentInfo>();
			List<PaymentEntity> payments = accountid.Data.ChargedPayments;
			foreach (PaymentEntity item in payments)
			{
				if (item.IsRecurring)
				{
					if (item.Type != Foundation.PaymentType.Income)
					{
						iReccurance reccurance = strategy[item.RecurringPayment.Recurrence]();
						allpayment.AddRange(reccurance.GetReccurance(item));
					}
				}
				else if (item.Type != Foundation.PaymentType.Income)
				{
					LiveTilesPaymentInfo tileinfo = new LiveTilesPaymentInfo();
					tileinfo.Chargeaccountname = item.ChargedAccount.Name;
					tileinfo.Myamount = item.Amount;
					tileinfo.Mydate = item.Date.Date;
					tileinfo.Type = item.Type;
					allpayment.Add(tileinfo);
				}
			}
			List<LiveTilesPaymentInfo> tiles = allpayment.Where(x => x.Mydate.Date.Month == month && x.Mydate.Date.Year == year).ToList();

			foreach (LiveTilesPaymentInfo item in tiles)
			{
				balance += item.Myamount;
			}
			allpayment.Clear();
			return balance;
		}

		public static async Task<List<string>> GetPreviouspaymentsAsync(string tilesize,int takeamount=8)
		{
			reccuringPaymentIds.Clear();
			DateTime today = DateTime.Now.Date;
			List<Account> acct = (await  accountService.GetAllAccounts()).ToList();
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
						iReccurance reccurance = strategy[item.RecurringPayment.Recurrence]();
						allpayment.AddRange(reccurance.GetReccurance(item));
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
			List<string> returnlist = new List<string>();
			
			List<LiveTilesPaymentInfo> lastmonth = allpayment.OrderByDescending(x=>x.Mydate.Date).ThenBy( x=> x.Mydate.Date<= today.Date).Take(takeamount).ToList();
			int index = 0;
			foreach (var item in lastmonth)
			{
				//{0} from {1}
				if (item.Type == Foundation.PaymentType.Income)
				{
					switch (tilesize)
					{
						case "medium":
							returnlist.Add(item.Chargeaccountname + " +" + TruncateNumber(item.Myamount));
							break;
						case "wide":
						case "large":
							returnlist.Add(string.Format(GetResourceKey("LiveTileWideandLargeIncomePastText"), item.Myamount.ToString("C2"), item.Chargeaccountname, item.Mydate.Date));
							break;
						default:
							break;
					}

				}
				else
				{
					switch (tilesize)
					{
						case "medium":
							returnlist.Add(item.Chargeaccountname + " -" + TruncateNumber(item.Myamount));
							break;
						case "wide":
						case "large":
							returnlist.Add(string.Format(GetResourceKey("LiveTileWideandLargePaymentPastText"), item.Myamount.ToString("C2"), item.Chargeaccountname));
							break;
						default:
							break;
					}
				}
				index += 1;
			}
			for (int i = index; i < (takeamount-1); i++)
			{
				returnlist.Add(string.Empty);

			}
			allpayments.Clear();
			return returnlist;

		}

		public static async Task<List<string>> GetNextpaymentsAsync(string tilesize,int takeamount=8)
		{
			reccuringPaymentIds.Clear();
			DateTime today = DateTime.Now;
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

					iReccurance reccurance = strategy[item.RecurringPayment.Recurrence]();
					allpayment.AddRange(reccurance.GetReccurance(item));

				}
				else
				{
					LiveTilesPaymentInfo tileinfor = new LiveTilesPaymentInfo();
					tileinfor.Chargeaccountname = item.ChargedAccount.Name;
					tileinfor.Myamount = item.Amount;
					tileinfor.Mydate = item.Date.Date;
					tileinfor.Type = item.Type;
					allpayment.Add(tileinfor);
				}
			}
			List<LiveTilesPaymentInfo> payments = allpayment.OrderBy(x=>x.Mydate.Date).ThenBy(x=> x.Mydate.Date>=today).Take(takeamount).ToList();
			List<string> returnlist = new List<string>();
			int index = 0;
			foreach (var item in payments)
			{
				if (item.Type == Foundation.PaymentType.Income)
				{
					switch (tilesize)
					{
						case "medium":
							returnlist.Add(item.Chargeaccountname + " +" + TruncateNumber(item.Myamount));
							break;
						case "wide":
						case "large":
							returnlist.Add(string.Format(GetResourceKey("LiveTileWideandLargeIncomeFutureText"), item.Chargeaccountname,"+"+ item.Myamount.ToString("C2"), item.Mydate.Date));

							break;
						default:
							break;
					}
					
				}
				else
				{
					switch (tilesize)
					{
						case "medium":
							returnlist.Add(item.Chargeaccountname +" -"+ TruncateNumber(item.Myamount));

							break;
						case "wide":
						case "large":
							returnlist.Add(string.Format(GetResourceKey("LiveTileWideandLargePaymentFutureText"), item.Chargeaccountname,"-"+ item.Myamount.ToString("C2")));
							break;
						default:
							break;
					}
				}
				index += 1;
			}
		
				for (int i = index; i < (takeamount-1); i++)
				{
					returnlist.Add(string.Empty);

				}
			
			return returnlist;
		}

		public static async Task UpdatePrimaryLiveTile()
		{
			strategy.Clear();
			strategy.Add(Foundation.PaymentRecurrence.Daily, () => new RecurrDaily());
			strategy.Add(Foundation.PaymentRecurrence.DailyWithoutWeekend, () => new RecurrWeekdays());
			strategy.Add(Foundation.PaymentRecurrence.Weekly, () => new RecurrWeekly());
			strategy.Add(Foundation.PaymentRecurrence.Biweekly, () => new RecurrBiWeekly());
			strategy.Add(Foundation.PaymentRecurrence.Monthly, () => new RecurrMonthly());
			strategy.Add(Foundation.PaymentRecurrence.Bimonthly, () => new RecurrBiMonthly());
			strategy.Add(Foundation.PaymentRecurrence.Quarterly, () => new RecurrQuarterly());
			strategy.Add(Foundation.PaymentRecurrence.Yearly, () => new RecurrYearly());
			strategy.Add(Foundation.PaymentRecurrence.Biannually, () => new RecurrbiYearly());
			AppListEntry entry = (await Package.Current.GetAppListEntriesAsync())[0];
			bool isPinned = await StartScreenManager.GetDefault().ContainsAppListEntryAsync(entry);
			if (isPinned)
			{
				object b = Localsettings.Values["lastrun"];
				string lastrun = (string)b;
				string headertext ="";
				List<string> displaycontentmedium = new List<string>();
				List<string> displaycontentlarge = new List<string>();
				if (lastrun == "last")
				{
					Localsettings.Values["lastrun"] = "next";
					headertext = GetResourceKey("LiveTileUpcommingPayments");
					displaycontentmedium = await GetNextpaymentsAsync("medium",8);
					displaycontentlarge = await GetNextpaymentsAsync("large",8);
				}
				else
				{
					Localsettings.Values["lastrun"] = "last";
					headertext = GetResourceKey("LiveTilePastPayments");
					displaycontentmedium = await GetPreviouspaymentsAsync("medium",8);
					displaycontentlarge = await GetPreviouspaymentsAsync("large",8);
				}

				TileContent content = new TileContent()
				{
					Visual = new TileVisual()
					{
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
																		Text = headertext,
																		HintStyle = AdaptiveTextStyle.Caption
																	},
																	new AdaptiveText()
																	{
																		Text = displaycontentlarge[0],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																   new AdaptiveText()
																	{
																		Text = displaycontentlarge[1],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																	 new AdaptiveText()
																	{
																		Text = displaycontentlarge[2],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																	new AdaptiveText()
																	{
																		Text = displaycontentlarge[3],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																   new AdaptiveText()
																	{
																		Text = displaycontentlarge[4],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},new AdaptiveText()
																	{
																		Text = displaycontentlarge[5],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																	new AdaptiveText()
																	{
																		Text = displaycontentlarge[6],
																		HintStyle=AdaptiveTextStyle.CaptionSubtle
																	},
																   new AdaptiveText()
																	{
																		Text = displaycontentlarge[7],
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
																		Text = headertext,
																		HintStyle = AdaptiveTextStyle.Caption
																	},
																	new AdaptiveText()
																	{
																		Text = displaycontentlarge[0],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																   new AdaptiveText()
																	{
																		Text = displaycontentlarge[1],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																	 new AdaptiveText()
																	{
																		Text = displaycontentlarge[2],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																	new AdaptiveText()
																	{
																		Text = displaycontentlarge[3],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																   new AdaptiveText()
																	{
																		Text = displaycontentlarge[4],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},new AdaptiveText()
																	{
																		Text = displaycontentlarge[5],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																	new AdaptiveText()
																	{
																		Text = displaycontentlarge[6],
																		HintStyle=AdaptiveTextStyle.CaptionSubtle
																	},
																   new AdaptiveText()
																	{
																		Text = displaycontentlarge[7],
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
				TileUpdateManager.CreateTileUpdaterForApplication().Update(tn);
				

			}
			
		}

		public static async Task UpdateSecondaryLiveTiles()
		{
			strategy.Clear();
			strategy.Add(Foundation.PaymentRecurrence.Daily, () => new RecurrDaily());
			strategy.Add(Foundation.PaymentRecurrence.DailyWithoutWeekend, () => new RecurrWeekdays());
			strategy.Add(Foundation.PaymentRecurrence.Weekly, () => new RecurrWeekly());
			strategy.Add(Foundation.PaymentRecurrence.Biweekly, () => new RecurrBiWeekly());
			strategy.Add(Foundation.PaymentRecurrence.Monthly, () => new RecurrMonthly());
			strategy.Add(Foundation.PaymentRecurrence.Bimonthly, () => new RecurrBiMonthly());
			strategy.Add(Foundation.PaymentRecurrence.Quarterly, () => new RecurrQuarterly());
			strategy.Add(Foundation.PaymentRecurrence.Yearly, () => new RecurrYearly());
			strategy.Add(Foundation.PaymentRecurrence.Biannually, () => new RecurrbiYearly());
			var tiles = await SecondaryTile.FindAllForPackageAsync();
			List<string> displaycontent = new List<string>();
			displaycontent =await GetPreviouspaymentsAsync("large",6);
			if (tiles != null)
			{


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
												Text =  TruncateNumber(acct.Data.CurrentBalance),
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
													Text = string.Format(GetResourceKey("LiveTileAccountBalance"),acct.Data.CurrentBalance.ToString("C2")),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
												new AdaptiveText()
												{
												Text =  GetResourceKey("ExpenseLabel"),
												HintStyle = AdaptiveTextStyle.Caption
												},
												new AdaptiveText()
												{
													Text = string.Format(GetResourceKey("LiveTileLastMonthsExpenses"),System.Globalization.DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(DateTime.Now.AddMonths(-1).Month), TruncateNumber(GetMonthExpenses(DateTime.Now.AddMonths(-1).Month, DateTime.Now.Year,acct))),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
											   new AdaptiveText()
												{
													Text = string.Format(GetResourceKey("LiveTileCurrentMonthsExpenses"),System.Globalization.DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(DateTime.Now.Month),TruncateNumber(GetMonthExpenses(DateTime.Now.Month,DateTime.Now.Year,acct))),
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
													Text = string.Format(GetResourceKey("LiveTileAccountBalance"),acct.Data.CurrentBalance.ToString("C2")),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
												new AdaptiveText()
												{
												Text =  GetResourceKey("ExpenseLabel"),
												HintStyle = AdaptiveTextStyle.Caption
												},
												new AdaptiveText()
												{
													Text = string.Format(GetResourceKey("LiveTileLastMonthsExpenses"),System.Globalization.DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(DateTime.Now.AddMonths(-1).Month), GetMonthExpenses(DateTime.Now.AddMonths(-1).Month, DateTime.Now.Year,acct).ToString("C2")),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
											   new AdaptiveText()
												{
													Text = string.Format(GetResourceKey("LiveTileCurrentMonthsExpenses"),System.Globalization.DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(DateTime.Now.Month),GetMonthExpenses(DateTime.Now.Month,DateTime.Now.Year,acct).ToString("C2")),
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
													Text = string.Format(GetResourceKey("LiveTileAccountBalance"),acct.Data.CurrentBalance.ToString("C2")),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
												new AdaptiveText()
												{
												Text =  GetResourceKey("ExpenseLabel"),
												HintStyle = AdaptiveTextStyle.Caption
												},
												new AdaptiveText()
												{
													Text = string.Format(GetResourceKey("LiveTileLastMonthsExpenses"),System.Globalization.DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(DateTime.Now.AddMonths(-1).Month),GetMonthExpenses(DateTime.Now.AddMonths(-1).Month, DateTime.Now.Year,acct).ToString("C2")),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
											   new AdaptiveText()
												{
													Text = string.Format(GetResourceKey("LiveTileCurrentMonthsExpenses"),System.Globalization.DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(DateTime.Now.Month),GetMonthExpenses(DateTime.Now.Month,DateTime.Now.Year,acct).ToString("C2")),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
												new AdaptiveText()
												{
													Text = GetResourceKey("LiveTilePastPayments"),
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
			
		}

		public interface iReccurance
		{
			List<LiveTilesPaymentInfo> GetReccurance(PaymentEntity payment);
		}

		public	class RecurrDaily : iReccurance
		{

			public List<LiveTilesPaymentInfo> GetReccurance(PaymentEntity payment)
			{
				int i = 1;
				DateTime today = DateTime.Now;

				List<LiveTilesPaymentInfo> allpayment = new List<LiveTilesPaymentInfo>();
				if (!reccuringPaymentIds.Contains(payment.RecurringPaymentId.Value))
				{
					reccuringPaymentIds.Add(payment.RecurringPaymentId.Value);
					if (payment.RecurringPayment.IsEndless)
					{
						DateTime startDate = payment.RecurringPayment.StartDate;
						while (i < 9)
						{
							LiveTilesPaymentInfo tilesPaymentInfo = new LiveTilesPaymentInfo();
							tilesPaymentInfo.Mydate = startDate.Date;
							tilesPaymentInfo.Myamount = payment.RecurringPayment.Amount;
							tilesPaymentInfo.Chargeaccountname = payment.RecurringPayment.ChargedAccount.Name;
							tilesPaymentInfo.Type = payment.RecurringPayment.Type;
							allpayment.Add(tilesPaymentInfo);

							if (DateTime.Compare(today, startDate) <= 0)
							{

								i += 1;
							}

							startDate = startDate.AddMonths(1);

						};
					}
					else
					{

						DateTime startDate = payment.RecurringPayment.StartDate;
						DateTime endDate = payment.RecurringPayment.EndDate.Value;
						while (DateTime.Compare(startDate, endDate) <= 0)
						{
							LiveTilesPaymentInfo liveTilesPaymentInfo = new LiveTilesPaymentInfo();
							liveTilesPaymentInfo.Mydate = startDate;
							liveTilesPaymentInfo.Myamount = payment.RecurringPayment.Amount;
							liveTilesPaymentInfo.Chargeaccountname = payment.RecurringPayment.ChargedAccount.Name;
							liveTilesPaymentInfo.Type = payment.RecurringPayment.Type;
							allpayment.Add(liveTilesPaymentInfo);
							startDate = startDate.AddDays(1);

						};
					}
				}
				return allpayment;
			}
		}

		public class RecurrWeekdays : iReccurance
		{
			public List<LiveTilesPaymentInfo> GetReccurance(PaymentEntity payment)
			{
				int i = 1;
				DateTime today = DateTime.Now;

				List<LiveTilesPaymentInfo> allpayment = new List<LiveTilesPaymentInfo>();
				if (!reccuringPaymentIds.Contains(payment.RecurringPaymentId.Value))
				{
					reccuringPaymentIds.Add(payment.RecurringPaymentId.Value);
					if (payment.RecurringPayment.IsEndless)
					{
						DateTime startDate = payment.RecurringPayment.StartDate;
						while (i < 9)
						{

							if (startDate.DayOfWeek == DayOfWeek.Saturday || startDate.DayOfWeek == DayOfWeek.Sunday)
							{
								startDate = startDate.AddDays(1);
								continue;
							}
							else
							{
								LiveTilesPaymentInfo liveTilesPaymentInfo = new LiveTilesPaymentInfo();
								liveTilesPaymentInfo.Mydate = startDate;
								liveTilesPaymentInfo.Myamount = payment.RecurringPayment.Amount;
								liveTilesPaymentInfo.Chargeaccountname = payment.RecurringPayment.ChargedAccount.Name;
								liveTilesPaymentInfo.Type = payment.RecurringPayment.Type;
								allpayment.Add(liveTilesPaymentInfo);
								startDate = startDate.AddDays(1);
								if (DateTime.Compare(today,startDate)<=0)
								{
									i += 1;
								}
							}

							

						};
					}
					else
					{

						DateTime startDate = payment.RecurringPayment.StartDate;
						DateTime endDate = payment.RecurringPayment.EndDate.Value;
						while (DateTime.Compare(startDate, endDate) <= 0)
						{
						
							if (startDate.DayOfWeek == DayOfWeek.Saturday && startDate.DayOfWeek == DayOfWeek.Sunday)
							{
								startDate = startDate.AddDays(1);
								continue;

							}
							else
							{
								LiveTilesPaymentInfo liveTilesPaymentInfo = new LiveTilesPaymentInfo();
								liveTilesPaymentInfo.Mydate = startDate;
								liveTilesPaymentInfo.Myamount = payment.RecurringPayment.Amount;
								liveTilesPaymentInfo.Chargeaccountname = payment.RecurringPayment.ChargedAccount.Name;
								liveTilesPaymentInfo.Type = payment.RecurringPayment.Type;
								allpayment.Add(liveTilesPaymentInfo);
								startDate = startDate.AddDays(1);
								
							}
						};
					}
				}
				return allpayment;
			}
	
		}

		public 	class RecurrWeekly : iReccurance
		{

			public List<LiveTilesPaymentInfo> GetReccurance(PaymentEntity payment)
			{
				int i = 1;
				DateTime today = DateTime.Now;

				List<LiveTilesPaymentInfo> allpayment = new List<LiveTilesPaymentInfo>();
				if (!reccuringPaymentIds.Contains(payment.RecurringPaymentId.Value))
				{
					reccuringPaymentIds.Add(payment.RecurringPaymentId.Value);
					if (payment.RecurringPayment.IsEndless)
					{
						DateTime startDate = payment.RecurringPayment.StartDate;
						while (i < 9)
						{
							LiveTilesPaymentInfo liveTilesPaymentInfo = new LiveTilesPaymentInfo();
							liveTilesPaymentInfo.Mydate = startDate;
							liveTilesPaymentInfo.Myamount = payment.RecurringPayment.Amount;
							liveTilesPaymentInfo.Chargeaccountname = payment.RecurringPayment.ChargedAccount.Name;
							liveTilesPaymentInfo.Type = payment.RecurringPayment.Type;
							allpayment.Add(liveTilesPaymentInfo);
							

							if (DateTime.Compare(today, startDate) <= 0)
							{

								i += 1;
							}

							startDate = startDate.AddDays(7);

						};
					}
					else
					{

						DateTime startDate = payment.RecurringPayment.StartDate;
						DateTime endDate = payment.RecurringPayment.EndDate.Value;
						while (DateTime.Compare(startDate, endDate) <= 0)
						{
							LiveTilesPaymentInfo liveTilesPaymentInfo = new LiveTilesPaymentInfo();
							liveTilesPaymentInfo.Mydate = startDate;
							liveTilesPaymentInfo.Myamount = payment.RecurringPayment.Amount;
							liveTilesPaymentInfo.Chargeaccountname = payment.RecurringPayment.ChargedAccount.Name;
							liveTilesPaymentInfo.Type = payment.RecurringPayment.Type;
							allpayment.Add(liveTilesPaymentInfo);
							


							startDate = startDate.AddDays(7);

						};
					}
				}
				return allpayment;
			}
		

		}

		public class RecurrBiWeekly : iReccurance
		{
			public List<LiveTilesPaymentInfo> GetReccurance(PaymentEntity payment)
			{
				int i = 1;
				DateTime today = DateTime.Now;

				List<LiveTilesPaymentInfo> allpayment = new List<LiveTilesPaymentInfo>();
				if (!reccuringPaymentIds.Contains(payment.RecurringPaymentId.Value))
				{
					reccuringPaymentIds.Add(payment.RecurringPaymentId.Value);
					if (payment.RecurringPayment.IsEndless)
					{
						DateTime startDate = payment.RecurringPayment.StartDate;
						while (i < 9)
						{
							LiveTilesPaymentInfo liveTilesPaymentInfo = new LiveTilesPaymentInfo();
							liveTilesPaymentInfo.Mydate = startDate;
							liveTilesPaymentInfo.Myamount = payment.RecurringPayment.Amount;
							liveTilesPaymentInfo.Chargeaccountname = payment.RecurringPayment.ChargedAccount.Name;
							liveTilesPaymentInfo.Type = payment.RecurringPayment.Type;
							allpayment.Add(liveTilesPaymentInfo);
						

							if (DateTime.Compare(today, startDate) <= 0)
							{

								i += 1;
							}

							startDate = startDate.AddDays(14);
						};
					}
					else
					{

						DateTime startDate = payment.RecurringPayment.StartDate;
						DateTime endDate = payment.RecurringPayment.EndDate.Value;
						while (DateTime.Compare(startDate, endDate) <= 0)
						{
							LiveTilesPaymentInfo liveTilesPaymentInfo = new LiveTilesPaymentInfo();
							liveTilesPaymentInfo.Mydate = startDate;
							liveTilesPaymentInfo.Myamount = payment.RecurringPayment.Amount;
							liveTilesPaymentInfo.Chargeaccountname = payment.RecurringPayment.ChargedAccount.Name;
							liveTilesPaymentInfo.Type = payment.RecurringPayment.Type;
							allpayment.Add(liveTilesPaymentInfo);



							startDate = startDate.AddDays (14);

						};
					}
				}
				return allpayment;
			}

			public List<PaymentEntity> GetFutureReccurance(PaymentEntity payment)
			{
				int i = 1;
				DateTime today = DateTime.Now;

				List<PaymentEntity> allpayment = new List<PaymentEntity>();
				if (!reccuringPaymentIds.Contains(payment.RecurringPaymentId.Value))
				{
					reccuringPaymentIds.Add(payment.RecurringPaymentId.Value);
					if (payment.RecurringPayment.IsEndless)
					{
						DateTime startDate = payment.RecurringPayment.StartDate;
						while (i < 9)
						{

							if (DateTime.Compare(today, startDate) <= 0)
							{
								payment.Date = startDate;
								payment.Amount = payment.RecurringPayment.Amount;
								payment.Type = payment.RecurringPayment.Type;
								payment.ChargedAccount = payment.RecurringPayment.ChargedAccount;
								allpayment.Add(payment);
								i += 1;
							}

							startDate = startDate.AddDays(14);

						};
					}
					else
					{

						DateTime startDate = payment.RecurringPayment.StartDate;
						DateTime endDate = payment.RecurringPayment.EndDate.Value;
						while (DateTime.Compare(startDate, endDate) <= 0)
						{
							payment.Date = startDate;
							if (DateTime.Compare(today, startDate) <= 0)
							{
								payment.Date = startDate;
								payment.Amount = payment.RecurringPayment.Amount;
								payment.Type = payment.RecurringPayment.Type;
								payment.ChargedAccount = payment.RecurringPayment.ChargedAccount;
								allpayment.Add(payment);

							}

							startDate = startDate.AddDays(14);

						};
					}
				}
				return allpayment;
			}

			public List<PaymentEntity> GetMonthlyReccurance(PaymentEntity payment, int month, int year)
			{

				DateTime today = DateTime.Now;
				int i = 1;
				List<PaymentEntity> allpayment = new List<PaymentEntity>();

				if (!reccuringPaymentIds.Contains(payment.RecurringPaymentId.Value))
				{
					reccuringPaymentIds.Add(payment.RecurringPaymentId.Value);
					if (payment.RecurringPayment.IsEndless)
					{
						DateTime startDate = payment.RecurringPayment.StartDate;
						while (startDate.Month <= month && startDate.Year <= year)
						{
							if (startDate.Month == month && startDate.Year == year)
							{
								payment.Date = startDate;
								payment.Amount = payment.RecurringPayment.Amount;
								payment.Type = payment.RecurringPayment.Type;
								payment.ChargedAccount = payment.RecurringPayment.ChargedAccount;
								allpayment.Add(payment);
								i += 1;
							}

							startDate = startDate.AddDays(14);

						};
					}
					else
					{
						DateTime startDate = payment.RecurringPayment.StartDate;
						DateTime endDate = payment.RecurringPayment.EndDate.Value;
						while (DateTime.Compare(startDate, endDate) <= 0)
						{

							if (startDate.Month == month && startDate.Year == year)
							{
								payment.Date = startDate;
								payment.Amount = payment.RecurringPayment.Amount;
								payment.Type = payment.RecurringPayment.Type;
								payment.ChargedAccount = payment.RecurringPayment.ChargedAccount;
								allpayment.Add(payment);
							}

							startDate = startDate.AddDays(14);

						};
					}
				}
				return allpayment;
			}



		}

		public class RecurrMonthly : iReccurance
		{
			
			public List<LiveTilesPaymentInfo> GetReccurance(PaymentEntity payment)
			{
				int i = 1;
				DateTime today = DateTime.Now;

				List<LiveTilesPaymentInfo> allpayment = new List<LiveTilesPaymentInfo>();
				if (!reccuringPaymentIds.Contains(payment.RecurringPaymentId.Value))
				{
					reccuringPaymentIds.Add(payment.RecurringPaymentId.Value);
					if (payment.RecurringPayment.IsEndless)
					{
						DateTime startDate = payment.RecurringPayment.StartDate;
						while (i < 9)
						{
							LiveTilesPaymentInfo liveTilesPaymentInfo = new LiveTilesPaymentInfo();
							liveTilesPaymentInfo.Mydate = startDate;
							liveTilesPaymentInfo.Myamount = payment.RecurringPayment.Amount;
							liveTilesPaymentInfo.Chargeaccountname = payment.RecurringPayment.ChargedAccount.Name;
							liveTilesPaymentInfo.Type = payment.RecurringPayment.Type;
							allpayment.Add(liveTilesPaymentInfo);

							if (DateTime.Compare(today, startDate) <= 0)
							{

								i += 1;
							}

							startDate = startDate.AddMonths(1);

						};
					}
					else
					{

						DateTime startDate = payment.RecurringPayment.StartDate;
						DateTime endDate = payment.RecurringPayment.EndDate.Value;
						while (DateTime.Compare(startDate, endDate) <= 0)
						{
							LiveTilesPaymentInfo liveTilesPaymentInfo = new LiveTilesPaymentInfo();
							liveTilesPaymentInfo.Mydate = startDate;
							liveTilesPaymentInfo.Myamount = payment.RecurringPayment.Amount;
							liveTilesPaymentInfo.Chargeaccountname = payment.RecurringPayment.ChargedAccount.Name;
							liveTilesPaymentInfo.Type = payment.RecurringPayment.Type;
							allpayment.Add(liveTilesPaymentInfo);



							startDate = startDate.AddMonths(1);

						};
					}
				}
				return allpayment;
			}
	

		}

		public class RecurrBiMonthly : iReccurance
		{
			public List<LiveTilesPaymentInfo> GetReccurance(PaymentEntity payment)
			{
				int i = 1;
				DateTime today = DateTime.Now;

				List<LiveTilesPaymentInfo> allpayment = new List<LiveTilesPaymentInfo>();
				if (!reccuringPaymentIds.Contains(payment.RecurringPaymentId.Value))
				{
					reccuringPaymentIds.Add(payment.RecurringPaymentId.Value);
					if (payment.RecurringPayment.IsEndless)
					{
						DateTime startDate = payment.RecurringPayment.StartDate;
						while (i < 9)
						{
							LiveTilesPaymentInfo liveTilesPaymentInfo = new LiveTilesPaymentInfo();
							liveTilesPaymentInfo.Mydate = startDate;
							liveTilesPaymentInfo.Myamount = payment.RecurringPayment.Amount;
							liveTilesPaymentInfo.Chargeaccountname = payment.RecurringPayment.ChargedAccount.Name;
							liveTilesPaymentInfo.Type = payment.RecurringPayment.Type;
							allpayment.Add(liveTilesPaymentInfo);

							if (DateTime.Compare(today, startDate) <= 0)
							{

								i += 1;
							}

							startDate = startDate.AddMonths(2);

						};
					}
					else
					{

						DateTime startDate = payment.RecurringPayment.StartDate;
						DateTime endDate = payment.RecurringPayment.EndDate.Value;
						while (DateTime.Compare(startDate, endDate) <= 0)
						{
							LiveTilesPaymentInfo liveTilesPaymentInfo = new LiveTilesPaymentInfo();
							liveTilesPaymentInfo.Mydate = startDate;
							liveTilesPaymentInfo.Myamount = payment.RecurringPayment.Amount;
							liveTilesPaymentInfo.Chargeaccountname = payment.RecurringPayment.ChargedAccount.Name;
							liveTilesPaymentInfo.Type = payment.RecurringPayment.Type;
							allpayment.Add(liveTilesPaymentInfo);



							startDate = startDate.AddMonths(2);

						};
					}
				}
				return allpayment;
			}
	
		}

		public class RecurrQuarterly : iReccurance
		{
			public List<LiveTilesPaymentInfo> GetReccurance(PaymentEntity payment)
			{
				int i = 1;
				DateTime today = DateTime.Now;

				List<LiveTilesPaymentInfo> allpayment = new List<LiveTilesPaymentInfo>();
				if (!reccuringPaymentIds.Contains(payment.RecurringPaymentId.Value))
				{
					reccuringPaymentIds.Add(payment.RecurringPaymentId.Value);
					if (payment.RecurringPayment.IsEndless)
					{
						DateTime startDate = payment.RecurringPayment.StartDate;
						while (i < 9)
						{
							LiveTilesPaymentInfo liveTilesPaymentInfo = new LiveTilesPaymentInfo();
							liveTilesPaymentInfo.Mydate = startDate;
							liveTilesPaymentInfo.Myamount = payment.RecurringPayment.Amount;
							liveTilesPaymentInfo.Chargeaccountname = payment.RecurringPayment.ChargedAccount.Name;
							liveTilesPaymentInfo.Type = payment.RecurringPayment.Type;
							allpayment.Add(liveTilesPaymentInfo);

							if (DateTime.Compare(today, startDate) <= 0)
							{

								i += 1;
							}

							startDate = startDate.AddMonths(3);

						};
					}
					else
					{

						DateTime startDate = payment.RecurringPayment.StartDate;
						DateTime endDate = payment.RecurringPayment.EndDate.Value;
						while (DateTime.Compare(startDate, endDate) <= 0)
						{
							LiveTilesPaymentInfo liveTilesPaymentInfo = new LiveTilesPaymentInfo();
							liveTilesPaymentInfo.Mydate = startDate;
							liveTilesPaymentInfo.Myamount = payment.RecurringPayment.Amount;
							liveTilesPaymentInfo.Chargeaccountname = payment.RecurringPayment.ChargedAccount.Name;
							liveTilesPaymentInfo.Type = payment.RecurringPayment.Type;
							allpayment.Add(liveTilesPaymentInfo);



							startDate = startDate.AddMonths(3);

						};
					}
				}
				return allpayment;
			}
	
		}

		public class RecurrYearly : iReccurance
		{
			public List<LiveTilesPaymentInfo> GetReccurance(PaymentEntity payment)
			{
				int i = 1;
				DateTime today = DateTime.Now;

				List<LiveTilesPaymentInfo> allpayment = new List<LiveTilesPaymentInfo>();
				if (!reccuringPaymentIds.Contains(payment.RecurringPaymentId.Value))
				{
					reccuringPaymentIds.Add(payment.RecurringPaymentId.Value);
					if (payment.RecurringPayment.IsEndless)
					{
						DateTime startDate = payment.RecurringPayment.StartDate;
						while (i < 9)
						{
							LiveTilesPaymentInfo liveTilesPaymentInfo = new LiveTilesPaymentInfo();
							liveTilesPaymentInfo.Mydate = startDate;
							liveTilesPaymentInfo.Myamount = payment.RecurringPayment.Amount;
							liveTilesPaymentInfo.Chargeaccountname = payment.RecurringPayment.ChargedAccount.Name;
							liveTilesPaymentInfo.Type = payment.RecurringPayment.Type;
							allpayment.Add(liveTilesPaymentInfo);

							if (DateTime.Compare(today, startDate) <= 0)
							{

								i += 1;
							}

							startDate = startDate.AddMonths(12);

						};
					}
					else
					{

						DateTime startDate = payment.RecurringPayment.StartDate;
						DateTime endDate = payment.RecurringPayment.EndDate.Value;
						while (DateTime.Compare(startDate, endDate) <= 0)
						{
							LiveTilesPaymentInfo liveTilesPaymentInfo = new LiveTilesPaymentInfo();
							liveTilesPaymentInfo.Mydate = startDate;
							liveTilesPaymentInfo.Myamount = payment.RecurringPayment.Amount;
							liveTilesPaymentInfo.Chargeaccountname = payment.RecurringPayment.ChargedAccount.Name;
							liveTilesPaymentInfo.Type = payment.RecurringPayment.Type;
							allpayment.Add(liveTilesPaymentInfo);



							startDate = startDate.AddMonths(12);

						};
					}
				}
				return allpayment;
			}
	

		}

		public	class RecurrbiYearly : iReccurance
		{

			public List<LiveTilesPaymentInfo> GetReccurance(PaymentEntity payment)
			{
				int i = 1;
				DateTime today = DateTime.Now;

				List<LiveTilesPaymentInfo> allpayment = new List<LiveTilesPaymentInfo>();
				if (!reccuringPaymentIds.Contains(payment.RecurringPaymentId.Value))
				{
					reccuringPaymentIds.Add(payment.RecurringPaymentId.Value);
					if (payment.RecurringPayment.IsEndless)
					{
						DateTime startDate = payment.RecurringPayment.StartDate;
						while (i < 9)
						{
							LiveTilesPaymentInfo liveTilesPaymentInfo = new LiveTilesPaymentInfo();
							liveTilesPaymentInfo.Mydate = startDate;
							liveTilesPaymentInfo.Myamount = payment.RecurringPayment.Amount;
							liveTilesPaymentInfo.Chargeaccountname = payment.RecurringPayment.ChargedAccount.Name;
							liveTilesPaymentInfo.Type = payment.RecurringPayment.Type;
							allpayment.Add(liveTilesPaymentInfo);

							if (DateTime.Compare(today, startDate) <= 0)
							{
								
								i += 1;
							}

							startDate = startDate.AddMonths(24);

						};
					}
					else
					{

						DateTime startDate = payment.RecurringPayment.StartDate;
						DateTime endDate = payment.RecurringPayment.EndDate.Value;
						while (DateTime.Compare(startDate, endDate) <= 0)
						{
							LiveTilesPaymentInfo liveTilesPaymentInfo = new LiveTilesPaymentInfo();
							liveTilesPaymentInfo.Mydate = startDate;
							liveTilesPaymentInfo.Myamount = payment.RecurringPayment.Amount;
							liveTilesPaymentInfo.Chargeaccountname = payment.RecurringPayment.ChargedAccount.Name;
							liveTilesPaymentInfo.Type = payment.RecurringPayment.Type;
							allpayment.Add(liveTilesPaymentInfo);



							startDate = startDate.AddMonths(24);

						};
					}
				}
				return allpayment;
			}
		

		}
	
	}
	public class LiveTilesPaymentInfo
	{
		public DateTime Mydate { get; set; }
		public double Myamount { get; set; }
		public PaymentType Type { get; set; }
		public string Chargeaccountname { get; set; }
	}
}
