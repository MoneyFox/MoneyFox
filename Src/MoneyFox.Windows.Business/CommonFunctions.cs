using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.DbContextScope;
using Microsoft.Toolkit.Uwp.Notifications;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation.Resources;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.VoiceCommands;
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

		public static string GetResourceKey(string keytofind)
		{
			System.Resources.ResourceManager keyValuePairs = Strings.ResourceManager;
			return keyValuePairs.GetString(keytofind);
		}

		public static double GetMonthExpenses(int month, int year, Account accountid)
		{
			reccuringPaymentIds.Clear();
			double balance = 0.00;
			List<PaymentEntity> allpayment = new List<PaymentEntity>();
			List<PaymentEntity> payments = accountid.Data.ChargedPayments;
			foreach (PaymentEntity item in payments)
			{
				if (item.IsRecurring)
				{
					if (item.Type != Foundation.PaymentType.Income)
					{
						iReccurance reccurance = strategy[item.RecurringPayment.Recurrence]();
						allpayment.AddRange(reccurance.GetMonthlyReccurance(item,month,year));
					}
				}
				else if (item.Type != Foundation.PaymentType.Income && item.Date.Month==month && item.Date.Year==year)
				{
				  allpayment.Add(item);
				}
			}
			foreach (PaymentEntity item in allpayment)
			{
				balance += item.Amount;
			}
			//allpayment.Clear();
			return balance;
		}

		public static async Task<List<string>> GetPreviouspaymentsAsync(int takeamount=8)
		{
			reccuringPaymentIds.Clear();
			var acct = await accountService.GetAllAccounts();
			List<PaymentEntity> allpayments = new List<PaymentEntity>();
			foreach (Account item in acct)
			{
				if (item.Data.ChargedPayments != null)
				{
					allpayments.AddRange(item.Data.ChargedPayments);
				}
				if (item.Data.TargetedPayments != null)
				{
					allpayments.AddRange(item.Data.TargetedPayments);
				}
			}
		IEnumerable<PaymentEntity> lastmonth = allpayments.OrderByDescending(x=> x.Date).Take(takeamount);
			List<string> returnlist = new List<string>();
			foreach (var item in lastmonth)
			{
			
				if (item.Type == Foundation.PaymentType.Income)
				{
					returnlist.Add(string.Format(GetResourceKey("LiveTilePaymentText"), item.Amount.ToString("C2")));
				}
				else
				{
					returnlist.Add(string.Format(GetResourceKey("LiveTilePaymentText"),"-"+ item.Amount.ToString("C2")));

				}
			}
			allpayments.Clear();
			return returnlist;

		}

		public static async Task<List<string>> GetNextpaymentsAsync(int takeamount=8)
		{
			reccuringPaymentIds.Clear();
			DateTime today = DateTime.Now;
			var acct = await accountService.GetAllAccounts();
			List<PaymentEntity> allpayments = new List<PaymentEntity>();
			List<PaymentEntity> allpayment = new List<PaymentEntity>();

			foreach (Account item in acct)
			{
				if (item.Data.ChargedPayments != null)
				{
					allpayments.AddRange(item.Data.ChargedPayments);
				}
				if (item.Data.TargetedPayments != null)
				{
					allpayments.AddRange(item.Data.TargetedPayments);
				}

			}
			foreach (PaymentEntity item in allpayments)
			{
				if (item.IsRecurring)
				{
					
						iReccurance reccurance = strategy[item.RecurringPayment.Recurrence]();
						allpayment.AddRange(reccurance.GetFutureReccurance(item));
					
				}
				else if (DateTime.Compare(today,item.Date)<=0)
				{
				allpayment.Add(item);
				}
			}
			IEnumerable<PaymentEntity> payments = allpayment.OrderBy(x => x.Date).Take(takeamount);
			List<string> returnlist = new List<string>();
			foreach (var item in payments)
			{
			
				if (item.Type == Foundation.PaymentType.Income)
				{
					returnlist.Add(string.Format(GetResourceKey("LiveTilePaymentText"), item.Amount.ToString("C2")));
				}
				else
				{
					returnlist.Add(string.Format(GetResourceKey("LiveTilePaymentText"),"-" +  item.Amount.ToString("C2")));

				}
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
				List<string> displaycontent = new List<string>();
				if (lastrun == "last")
				{
					Localsettings.Values["lastrun"] = "next";
					headertext = GetResourceKey("LiveTileUpcommingPayments");
					displaycontent = await GetNextpaymentsAsync();
				}
				else
				{
					Localsettings.Values["lastrun"] = "last";
					headertext = GetResourceKey("LiveTilePastPayments");
					displaycontent = await GetPreviouspaymentsAsync();
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
																		Text = (displaycontent.Count()==1)?displaycontent[0]:string.Empty,
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																   new AdaptiveText()
																	{
																		Text = (displaycontent.Count()==2)?displaycontent[1]:string.Empty,
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																	 new AdaptiveText()
																	{
																		Text = (displaycontent.Count()==3)?displaycontent[2]:string.Empty,
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																	new AdaptiveText()
																	{
																		Text = (displaycontent.Count()==4)?displaycontent[3]:string.Empty,
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																   new AdaptiveText()
																	{
																		Text = (displaycontent.Count()==5)?displaycontent[4]:string.Empty,
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},new AdaptiveText()
																	{
																		Text = (displaycontent.Count()==6)?displaycontent[5]:string.Empty,
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																	new AdaptiveText()
																	{
																		Text = (displaycontent.Count()==7)?displaycontent[6]:string.Empty,
																		HintStyle=AdaptiveTextStyle.CaptionSubtle
																	},
																   new AdaptiveText()
																	{
																		Text = (displaycontent.Count()==8)?displaycontent[7]:string.Empty,
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
																		Text = (displaycontent.Count()==1)?displaycontent[0]:string.Empty,
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																   new AdaptiveText()
																	{
																		Text = (displaycontent.Count()==2)?displaycontent[1]:string.Empty,
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																	 new AdaptiveText()
																	{
																		Text = (displaycontent.Count()==3)?displaycontent[2]:string.Empty,
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																	new AdaptiveText()
																	{
																		Text = (displaycontent.Count()==4)?displaycontent[3]:string.Empty,
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																   new AdaptiveText()
																	{
																		Text = (displaycontent.Count()==5)?displaycontent[4]:string.Empty,
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},new AdaptiveText()
																	{
																		Text = (displaycontent.Count()==6)?displaycontent[5]:string.Empty,
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																	new AdaptiveText()
																	{
																		Text = (displaycontent.Count()==7)?displaycontent[6]:string.Empty,
																		HintStyle=AdaptiveTextStyle.CaptionSubtle
																	},
																   new AdaptiveText()
																	{
																		Text = (displaycontent.Count()==8)?displaycontent[7]:string.Empty,
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
																		Text = (displaycontent.Count()==1)?displaycontent[0]:string.Empty,
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																   new AdaptiveText()
																	{
																		Text = (displaycontent.Count()==2)?displaycontent[1]:string.Empty,
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																	 new AdaptiveText()
																	{
																		Text = (displaycontent.Count()==3)?displaycontent[2]:string.Empty,
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																	new AdaptiveText()
																	{
																		Text = (displaycontent.Count()==4)?displaycontent[3]:string.Empty,
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																   new AdaptiveText()
																	{
																		Text = (displaycontent.Count()==5)?displaycontent[4]:string.Empty,
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},new AdaptiveText()
																	{
																		Text = (displaycontent.Count()==6)?displaycontent[5]:string.Empty,
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																	new AdaptiveText()
																	{
																		Text = (displaycontent.Count()==7)?displaycontent[6]:string.Empty,
																		HintStyle=AdaptiveTextStyle.CaptionSubtle
																	},
																   new AdaptiveText()
																	{
																		Text = (displaycontent.Count()==8)?displaycontent[7]:string.Empty,
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
												Text =  acct.Data.CurrentBalance.ToString("C2"),
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
			List<PaymentEntity> GetMonthlyReccurance(PaymentEntity payment, int month, int year);
			List<PaymentEntity> GetFutureReccurance(PaymentEntity payment);
		}

		public	class RecurrDaily : iReccurance
		{
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

							startDate = startDate.AddDays(1);

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

							startDate = startDate.AddDays(1);

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

							startDate = startDate.AddDays(1);

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

							startDate = startDate.AddDays(1);

						};
					}
				}
				return allpayment;
			}
		}

		public class RecurrWeekdays : iReccurance
		{
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

							if (DateTime.Compare(today, startDate) <= 0 && startDate.DayOfWeek!=DayOfWeek.Saturday && startDate.DayOfWeek!=DayOfWeek.Sunday)
							{
								payment.Date = startDate;
								payment.Amount = payment.RecurringPayment.Amount;
								payment.Type = payment.RecurringPayment.Type;
								payment.ChargedAccount = payment.RecurringPayment.ChargedAccount;
								allpayment.Add(payment);
								i += 1;
							}

							startDate = startDate.AddDays(1);

						};
					}
					else
					{

						DateTime startDate = payment.RecurringPayment.StartDate;
						DateTime endDate = payment.RecurringPayment.EndDate.Value;
						while (DateTime.Compare(startDate, endDate) <= 0)
						{
							payment.Date = startDate;
							if (DateTime.Compare(today, startDate) <= 0 && startDate.DayOfWeek != DayOfWeek.Saturday && startDate.DayOfWeek != DayOfWeek.Sunday)
							{
								payment.Date = startDate;
								payment.Amount = payment.RecurringPayment.Amount;
								payment.Type = payment.RecurringPayment.Type;
								payment.ChargedAccount = payment.RecurringPayment.ChargedAccount;
								allpayment.Add(payment);

							}

							startDate = startDate.AddDays(1);

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
						while (startDate.Month <= month && startDate.Year <= year )
						{
							if (startDate.Month == month && startDate.Year == year && startDate.DayOfWeek != DayOfWeek.Saturday && startDate.DayOfWeek != DayOfWeek.Sunday)
							{
								payment.Date = startDate;
								payment.Amount = payment.RecurringPayment.Amount;
								payment.Type = payment.RecurringPayment.Type;
								payment.ChargedAccount = payment.RecurringPayment.ChargedAccount;
								allpayment.Add(payment);
								i += 1;
							}

							startDate = startDate.AddDays(1);

						};
					}
					else
					{
						DateTime startDate = payment.RecurringPayment.StartDate;
						DateTime endDate = payment.RecurringPayment.EndDate.Value;
						while (DateTime.Compare(startDate, endDate) <= 0)
						{

							if (startDate.Month == month && startDate.Year == year && startDate.DayOfWeek != DayOfWeek.Saturday && startDate.DayOfWeek != DayOfWeek.Sunday)
							{
								payment.Date = startDate;
								payment.Amount = payment.RecurringPayment.Amount;
								payment.Type = payment.RecurringPayment.Type;
								payment.ChargedAccount = payment.RecurringPayment.ChargedAccount;
								allpayment.Add(payment);
							}

							startDate = startDate.AddDays(1);

						};
					}
				}
				return allpayment;
			}

		}

		public 	class RecurrWeekly : iReccurance
		{
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

							startDate = startDate.AddDays(7);

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

							startDate = startDate.AddDays(7);

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

							startDate = startDate.AddDays(7);

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

							startDate = startDate.AddDays(7);

						};
					}
				}
				return allpayment;
			}



		}

		public class RecurrBiWeekly : iReccurance
		{
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

							startDate = startDate.AddMonths(1);

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

							startDate = startDate.AddMonths(1);

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

							startDate = startDate.AddMonths(1);

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

							startDate = startDate.AddMonths(1);

						};
					}
				}
				return allpayment;
			}


		}

		public class RecurrBiMonthly : iReccurance
		{
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

							startDate = startDate.AddMonths(2);

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

							startDate = startDate.AddMonths(2);

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

							startDate = startDate.AddMonths(2);

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

							startDate = startDate.AddMonths(2);

						};
					}
				}
				return allpayment;
			}


		}

		public class RecurrQuarterly : iReccurance
		{
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

							startDate = startDate.AddMonths(4);

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

							startDate = startDate.AddMonths(4);

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

							startDate = startDate.AddMonths(4);

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

							startDate = startDate.AddMonths(4);

						};
					}
				}
				return allpayment;
			}



		}

		public class RecurrYearly : iReccurance
		{
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

							startDate = startDate.AddMonths(12);

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

							startDate = startDate.AddMonths(12);

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

							startDate = startDate.AddMonths(12);

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

							startDate = startDate.AddMonths(12);

						};
					}
				}
				return allpayment;
			}



		}

		public	class RecurrbiYearly : iReccurance
		{
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

							startDate = startDate.AddMonths(24);

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

							startDate = startDate.AddMonths(24);

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

							startDate = startDate.AddMonths(24);

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

							startDate = startDate.AddMonths(24);

						};
					}
				}
				return allpayment;
			}


		}
	
	}
}
