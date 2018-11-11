using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.DbContextScope;
using Microsoft.Toolkit.Uwp.Notifications;
using MoneyFox.DataAccess.DataServices;
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

		private static List<CommonFunctions.Paymentitem> allpayment = new List<CommonFunctions.Paymentitem>();
		private static Dictionary<Foundation.PaymentRecurrence, Func<CommonFunctions.iReccurance>> strategy = new Dictionary<Foundation.PaymentRecurrence, Func<CommonFunctions.iReccurance>>();

		private static ApplicationDataContainer Localsettings = ApplicationData.Current.LocalSettings;
		public static string GetResourceKey(string keytofind)
		{
			System.Resources.ResourceManager keyValuePairs = Strings.ResourceManager;
			return keyValuePairs.GetString(keytofind);
		}

		public static SecondaryTile CreateSecondaryTile(int id)
		{
		
		}
		public static async Task<double> GetMonthExpensesAsync(int month, int accountid)
		{
			var currentmonth = DateTime.Now.Month;
			double balance = 0.00;
			IEnumerable<Payment> payments = await paymentService.GetPaymentsByAccountId(accountid);
			foreach (Payment item in payments)
			{
				if (item.Data.IsRecurring)
				{
					if (item.Data.RecurringPayment.Type != Foundation.PaymentType.Income)
					{
						iReccurance reccurance = strategy[item.Data.RecurringPayment.Recurrence]();
						allpayment.AddRange(reccurance.GetPastreccurance(item));
					}
				}
				else if(item.Data.Type!= Foundation.PaymentType.Income)
				{

					Paymentitem pay = new Paymentitem
					{
						dateTime = item.Data.Date,
						P = item
					};
				}
			}
			IEnumerable<Paymentitem> all = allpayment.Where(x => x.dateTime.Month == month && x.dateTime.Year == DateTime.Now.Year).ToList();
			foreach (Paymentitem item in all)
			{
				balance += item.P.Data.Amount;
			}
			allpayment.Clear();
			return balance;
		}

		public static async Task<List<string>> GetPreviouspaymentsAsync()
		{
			var acct = await accountService.GetAllAccounts();
			List<Payment> allpayments = new List<Payment>();
			foreach (Account item in acct)
			{
				allpayments.AddRange(await paymentService.GetPaymentsByAccountId(item.Data.Id));
			}
			foreach (Payment item in allpayments)
			{
				if (item.Data.IsRecurring)
				{

					
					
						iReccurance reccurance = strategy[item.Data.RecurringPayment.Recurrence]();
						allpayment.AddRange(reccurance.GetPastreccurance(item));
					
				}
				else
				{
					Paymentitem keyValuePair = new Paymentitem
					{
						dateTime = item.Data.Date,
						P = item
					};
					allpayment.Add(keyValuePair);
				}
			}
			IEnumerable<Paymentitem> tet = allpayment.OrderByDescending(x => x.dateTime).Take(8);
			List<string> returnlist = new List<string>();
			foreach (var item in tet)
			{
				Payment p = item.P;
				if (p.Data.Type == Foundation.PaymentType.Income)
				{
					returnlist.Add(string.Format(GetResourceKey("LiveTilePastIncomePaymentText"), p.Data.Amount, p.Data.ChargedAccount.Name, p.Data.Date))
				}
				else
				{
					returnlist.Add(string.Format(GetResourceKey("LiveTilePastPaymentText"), p.Data.Amount, p.Data.ChargedAccount.Name, p.Data.Date));
				}
			}
			allpayment.Clear();
			return returnlist;

		}

		public static async Task<List<string>> GetNextpaymentsAsync()
		{
			var acct = await accountService.GetAllAccounts();
			List<Payment> allpayments = new List<Payment>();

			foreach (Account item in acct)
			{
				allpayments.AddRange(await paymentService.GetPaymentsByAccountId(item.Data.Id));
			}
			foreach (Payment item in allpayments)
			{
				if (item.Data.IsRecurring)
				{
					
						iReccurance reccurance = strategy[item.Data.RecurringPayment.Recurrence]();
						allpayment.AddRange(reccurance.GetFutureReccurance(item));
					
				}
				else
				{
					Paymentitem pay = new Paymentitem
					{
						dateTime = item.Data.Date,
						P = item
					};
					allpayment.Add(pay);
				}
			}
			IEnumerable<Paymentitem> tet = allpayment.OrderBy(x => x.dateTime).Take(8);
			List<string> returnlist = new List<string>();
			foreach (var item in tet)
			{
				Payment P = item.P;
				if (P.Data.Type == Foundation.PaymentType.Income)
				{
					returnlist.Add(string.Format(GetResourceKey("LiveTileFutureIncomePaymentText"), P.Data.Amount, P.Data.ChargedAccount.Name, P.Data.Date))
				}
				else
				{
					returnlist.Add(string.Format(GetResourceKey("LiveTileFuturePaymentText"), P.Data.Amount, P.Data.ChargedAccount.Name, P.Data.Date));
				}
			}
			allpayment.Clear();


			return returnlist;

		}

		public static async Task<double> GetLatestBalanceAsync(int tileid)
		{
			var te = await accountService.GetById(tileid);
			return te.Data.CurrentBalance;
		}

		public static async Task UpdatePrimaryLiveTile()
		{
			AppListEntry entry = (await Package.Current.GetAppListEntriesAsync())[0];
			bool isPinned = await StartScreenManager.GetDefault().ContainsAppListEntryAsync(entry);
			if (isPinned)
			{
				object b = Localsettings.Values["lastrun"];
				string lastrun = (string)b;
				string nextpayment = GetResourceKey("LiveTileUpcommingPayments");
				string previouspayment = GetResourceKey("LiveTilePastPayments");
				if (lastrun == "last")
				{
					Localsettings.Values["lastrun"] = "next";

				}
				else
				{
					Localsettings.Values["lastrun"] = "last";

				}
				List<string> next = await GetNextpaymentsAsync();
				List<string> previous = await GetPreviouspaymentsAsync();
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
																		Text = (lastrun=="last")?nextpayment:previouspayment,
																		HintStyle = AdaptiveTextStyle.Caption
																	},
																	new AdaptiveText()
																	{
																		Text = (lastrun=="last")?previous[0]:next[0],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																   new AdaptiveText()
																	{
																		Text = (lastrun=="last")?previous[1]:next[1],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																	new AdaptiveText()
																	{
																		Text = (lastrun=="last")?previous[2]:next[2],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																	new AdaptiveText()
																	{
																		Text = (lastrun=="last")?previous[3]:next[3],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																   new AdaptiveText()
																	{
																		Text =(lastrun=="last")?previous[4]:next[4],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},new AdaptiveText()
																	{
																		Text = (lastrun=="last")?previous[5]:next[5],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																	new AdaptiveText()
																	{
																		Text = (lastrun=="last")?previous[6]:next[6],
																		HintStyle=AdaptiveTextStyle.CaptionSubtle
																	},
																   new AdaptiveText()
																	{
																		Text = (lastrun=="last")?previous[7]:next[7],
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
																		Text = (lastrun=="last")?nextpayment:previouspayment,
																		HintStyle = AdaptiveTextStyle.Caption
																	},
																	new AdaptiveText()
																	{
																		Text = (lastrun=="last")?previous[0]:next[0],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																   new AdaptiveText()
																	{
																		Text = (lastrun=="last")?previous[1]:next[1],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																	new AdaptiveText()
																	{
																		Text = (lastrun=="last")?previous[2]:next[2],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																	new AdaptiveText()
																	{
																		Text = (lastrun=="last")?previous[3]:next[3],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																   new AdaptiveText()
																	{
																		Text =(lastrun=="last")?previous[4]:next[4],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},new AdaptiveText()
																	{
																		Text = (lastrun=="last")?previous[5]:next[5],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																	new AdaptiveText()
																	{
																		Text = (lastrun=="last")?previous[6]:next[6],
																		HintStyle=AdaptiveTextStyle.CaptionSubtle
																	},
																   new AdaptiveText()
																	{
																		Text = (lastrun=="last")?previous[7]:next[7],
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
																		Text = (lastrun=="last")?nextpayment:previouspayment,
																		HintStyle = AdaptiveTextStyle.Caption
																	},
																	new AdaptiveText()
																	{
																		Text = (lastrun=="last")?previous[0]:next[0],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																   new AdaptiveText()
																	{
																		Text = (lastrun=="last")?previous[1]:next[1],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																	new AdaptiveText()
																	{
																		Text = (lastrun=="last")?previous[2]:next[2],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																	new AdaptiveText()
																	{
																		Text = (lastrun=="last")?previous[3]:next[3],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																   new AdaptiveText()
																	{
																		Text =(lastrun=="last")?previous[4]:next[4],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},new AdaptiveText()
																	{
																		Text = (lastrun=="last")?previous[5]:next[5],
																		HintStyle = AdaptiveTextStyle.CaptionSubtle
																	},
																	new AdaptiveText()
																	{
																		Text = (lastrun=="last")?previous[6]:next[6],
																		HintStyle=AdaptiveTextStyle.CaptionSubtle
																	},
																   new AdaptiveText()
																	{
																		Text = (lastrun=="last")?previous[7]:next[7],
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
			var tiles = await SecondaryTile.FindAllForPackageAsync();
			if (tiles != null)
			{


				foreach (SecondaryTile item in tiles)
				{
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
													Text = string.Format(GetResourceKey("LiveTileAccountName"),item.TileId),
													HintStyle = AdaptiveTextStyle.Caption
												},
											new AdaptiveText()
												{
													Text = string.Format(GetResourceKey("LiveTileAccountBalance"),await GetLatestBalanceAsync(int.Parse(item.TileId))),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
												new AdaptiveText()
												{
													Text = string.Format(GetResourceKey("LiveTileAccountLastMonthsExpenses"),await GetMonthExpensesAsync(DateTime.Now.AddMonths(-1).Month,int.Parse(item.TileId))),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
											   new AdaptiveText()
												{
													Text = string.Format(GetResourceKey("LiveTileAccountCurrentMonthsExpenses"), await GetMonthExpensesAsync(DateTime.Now.Month,int.Parse(item.TileId))),
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
													Text = string.Format(GetResourceKey("LiveTileAccountName"),item.TileId),
													HintStyle = AdaptiveTextStyle.Caption
												},
											new AdaptiveText()
												{
													Text = string.Format(GetResourceKey("LiveTileAccountBalance"),await GetLatestBalanceAsync(int.Parse(item.TileId))),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
												new AdaptiveText()
												{
													Text = string.Format(GetResourceKey("LiveTileAccountLastMonthsExpenses"),await GetMonthExpensesAsync(DateTime.Now.AddMonths(-1).Month,int.Parse(item.TileId))),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
											   new AdaptiveText()
												{
													Text = string.Format(GetResourceKey("LiveTileAccountCurrentMonthsExpenses"), await GetMonthExpensesAsync(DateTime.Now.Month,int.Parse(item.TileId))),
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
													Text = string.Format(GetResourceKey("LiveTileAccountName"),item.TileId),
													HintStyle = AdaptiveTextStyle.Caption
												},
											new AdaptiveText()
												{
													Text = string.Format(GetResourceKey("LiveTileAccountBalance"),await GetLatestBalanceAsync(int.Parse(item.TileId))),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
												new AdaptiveText()
												{
													Text = string.Format(GetResourceKey("LiveTileAccountLastMonthsExpenses"),await GetMonthExpensesAsync(DateTime.Now.AddMonths(-1).Month,int.Parse(item.TileId))),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
											   new AdaptiveText()
												{
													Text = string.Format(GetResourceKey("LiveTileAccountCurrentMonthsExpenses"), await GetMonthExpensesAsync(DateTime.Now.Month,int.Parse(item.TileId))),
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
			List<Paymentitem> GetPastreccurance(Payment payment);
			List<Paymentitem> GetFutureReccurance(Payment payment);
		}
		public	class RecurrDaily : iReccurance
		{
			public List<Paymentitem> GetFutureReccurance(Payment payment)
			{
				int i = 1;
				DateTime dt = DateTime.Now;
				DateTime td = payment.Data.RecurringPayment.StartDate;
				List<Paymentitem> allpayment = new List<Paymentitem>();
				while (i < 9)
				{
					payment.Data.Date = td;
					payment.Data.ChargedAccount = payment.Data.RecurringPayment.ChargedAccount;
					payment.Data.Amount = payment.Data.RecurringPayment.Amount;
					payment.Data.Type = payment.Data.RecurringPayment.Type;
					if (DateTime.Compare(dt, td) <= 0)
					{
						Paymentitem pay = new Paymentitem
						{
							dateTime = td,
							P = payment
						};
						allpayment.Add(pay);
						i += 1;
					}

					td = td.AddDays(1);

				};
				return allpayment;
			}

			public List<Paymentitem> GetPastreccurance(Payment payment)
			{
				DateTime dt = DateTime.Now;
				DateTime td = payment.Data.RecurringPayment.StartDate;
				List<Paymentitem> allpayment = new List<Paymentitem>();
				while (DateTime.Compare(dt, td) >= 0)
				{
					payment.Data.Date = td;
					payment.Data.ChargedAccount = payment.Data.RecurringPayment.ChargedAccount;
					payment.Data.Amount = payment.Data.RecurringPayment.Amount;
					payment.Data.Type = payment.Data.RecurringPayment.Type;
					Paymentitem pay = new Paymentitem
					{
						dateTime = td,
						P = payment
					};
					allpayment.Add(pay);
					td = td.AddDays(1);
				};
				return allpayment;

			}
		}

		public class RecurrWeekdays : iReccurance
		{
			public List<Paymentitem> GetFutureReccurance(Payment payment)
			{
				DateTime dt = DateTime.Now;
				DateTime td = payment.Data.RecurringPayment.StartDate;
				List<Paymentitem> allpayment = new List<Paymentitem>();
				int i = 1;
				while (i < 9)
				{
					payment.Data.Date = td;
					payment.Data.ChargedAccount = payment.Data.RecurringPayment.ChargedAccount;
					payment.Data.Amount = payment.Data.RecurringPayment.Amount;
					payment.Data.Type = payment.Data.RecurringPayment.Type;
					if (payment.Data.Date.DayOfWeek != DayOfWeek.Saturday && payment.Data.Date.DayOfWeek != DayOfWeek.Sunday && DateTime.Compare(dt, td) <= 0)
					{
						Paymentitem pay = new Paymentitem
						{
							dateTime = td,
							P = payment
						};
						allpayment.Add(pay);
						td = td.AddDays(1);
						i += 1;
					}

					td = td.AddDays(1);

				};
				return allpayment;
			}

			public List<Paymentitem> GetPastreccurance(Payment payment)
			{
				DateTime dt = DateTime.Now;
				DateTime td = payment.Data.RecurringPayment.StartDate;
				List<Paymentitem> allpayment = new List<Paymentitem>();
				while (DateTime.Compare(dt, td) >= 0)
				{
					payment.Data.Date = td;
					payment.Data.ChargedAccount = payment.Data.RecurringPayment.ChargedAccount;
					payment.Data.Amount = payment.Data.RecurringPayment.Amount;
					payment.Data.Type = payment.Data.RecurringPayment.Type;
					if (payment.Data.Date.DayOfWeek != DayOfWeek.Saturday && payment.Data.Date.DayOfWeek != DayOfWeek.Sunday)
					{
						Paymentitem pay = new Paymentitem
						{
							dateTime = td,
							P = payment
						};
						allpayment.Add(pay);
						td = td.AddDays(1);
					}
					td = td.AddDays(1);
				};
				return allpayment;

			}


		}

		public 	class RecurrWeekly : iReccurance
		{
			public List<Paymentitem> GetFutureReccurance(Payment payment)
			{
				int i = 1;
				DateTime dt = DateTime.Now;
				DateTime td = payment.Data.RecurringPayment.StartDate;
				List<Paymentitem> allpayment = new List<Paymentitem>();
				while (i < 9)
				{
					payment.Data.Date = td;
					payment.Data.ChargedAccount = payment.Data.RecurringPayment.ChargedAccount;
					payment.Data.Amount = payment.Data.RecurringPayment.Amount;
					payment.Data.Type = payment.Data.RecurringPayment.Type;
					if (DateTime.Compare(dt, td) <= 0)
					{
						Paymentitem pay = new Paymentitem
						{
							dateTime = td,
							P = payment
						};
						allpayment.Add(pay);
						i += 1;
					}

					td = td.AddDays(7);

				};
				return allpayment;
			}

			public List<Paymentitem> GetPastreccurance(Payment payment)
			{
				DateTime dt = DateTime.Now;
				DateTime td = payment.Data.RecurringPayment.StartDate;
				List<Paymentitem> allpayment = new List<Paymentitem>();
				while (DateTime.Compare(dt, td) >= 0)
				{
					payment.Data.Date = td;
					payment.Data.ChargedAccount = payment.Data.RecurringPayment.ChargedAccount;
					payment.Data.Amount = payment.Data.RecurringPayment.Amount;
					payment.Data.Type = payment.Data.RecurringPayment.Type;
					Paymentitem pay = new Paymentitem
					{
						dateTime = td,
						P = payment
					};
					allpayment.Add(pay);
					td = td.AddDays(7);

				};
				return allpayment;
			}

		}

		public class RecurrBiWeekly : iReccurance
		{
			public List<Paymentitem> GetFutureReccurance(Payment payment)
			{
				int i = 1;
				DateTime dt = DateTime.Now;
				DateTime td = payment.Data.RecurringPayment.StartDate;
				List<Paymentitem> allpayment = new List<Paymentitem>();
				while (i < 9)
				{
					payment.Data.Date = td;
					payment.Data.ChargedAccount = payment.Data.RecurringPayment.ChargedAccount;
					payment.Data.Amount = payment.Data.RecurringPayment.Amount;
					payment.Data.Type = payment.Data.RecurringPayment.Type;
					if (DateTime.Compare(dt, td) <= 0)
					{
						Paymentitem pay = new Paymentitem
						{
							dateTime = td,
							P = payment
						};
						allpayment.Add(pay);
						i += 1;
					}
					td = td.AddDays(14);

				};
				return allpayment;
			}

			public List<Paymentitem> GetPastreccurance(Payment payment)
			{
				DateTime dt = DateTime.Now;
				DateTime td = payment.Data.RecurringPayment.StartDate;
				List<Paymentitem> allpayment = new List<Paymentitem>();
				while (DateTime.Compare(dt, td) >= 0)
				{
					payment.Data.Date = td;
					payment.Data.ChargedAccount = payment.Data.RecurringPayment.ChargedAccount;
					payment.Data.Amount = payment.Data.RecurringPayment.Amount;
					payment.Data.Type = payment.Data.RecurringPayment.Type;
					Paymentitem pay = new Paymentitem
					{
						dateTime = td,
						P = payment
					};
					allpayment.Add(pay);
					td = td.AddDays(14);

				};
				return allpayment;
			}

		}

		public class RecurrMonthly : iReccurance
		{
			public List<Paymentitem> GetFutureReccurance(Payment payment)
			{
				int i = 1;
				DateTime dt = DateTime.Now;
				DateTime td = payment.Data.RecurringPayment.StartDate;
				List<Paymentitem> allpayment = new List<Paymentitem>();
				while (i < 9)
				{
					payment.Data.Date = td;
					payment.Data.ChargedAccount = payment.Data.RecurringPayment.ChargedAccount;
					payment.Data.Amount = payment.Data.RecurringPayment.Amount;
					payment.Data.Type = payment.Data.RecurringPayment.Type;
					if (DateTime.Compare(dt, td) <= 0)
					{
						Paymentitem pay = new Paymentitem
						{
							dateTime = td,
							P = payment
						};
						allpayment.Add(pay);
						i += 1;
					}
					td = td.AddMonths(1);

				};
				return allpayment;
			}

			public List<Paymentitem> GetPastreccurance(Payment payment)
			{
				DateTime dt = DateTime.Now;
				DateTime td = payment.Data.RecurringPayment.StartDate;
				List<Paymentitem> allpayment = new List<Paymentitem>();
				while (DateTime.Compare(dt, td) >= 0)
				{
					payment.Data.Date = td;
					payment.Data.ChargedAccount = payment.Data.RecurringPayment.ChargedAccount;
					payment.Data.Amount = payment.Data.RecurringPayment.Amount;
					payment.Data.Type = payment.Data.RecurringPayment.Type;
					Paymentitem pay = new Paymentitem
					{
						dateTime = td,
						P = payment
					};
					allpayment.Add(pay);
					td = td.AddMonths(1);

				};
				return allpayment;
			}

		}

		public class RecurrBiMonthly : iReccurance
		{
			public List<Paymentitem> GetFutureReccurance(Payment payment)
			{
				int i = 1;
				DateTime dt = DateTime.Now;
				DateTime td = payment.Data.RecurringPayment.StartDate;
				List<Paymentitem> allpayment = new List<Paymentitem>();
				while (i < 9)
				{
					payment.Data.Date = td;
					payment.Data.ChargedAccount = payment.Data.RecurringPayment.ChargedAccount;
					payment.Data.Amount = payment.Data.RecurringPayment.Amount;
					payment.Data.Type = payment.Data.RecurringPayment.Type;
					if (DateTime.Compare(dt, td) <= 0)
					{
						Paymentitem pay = new Paymentitem
						{
							dateTime = td,
							P = payment
						};
						allpayment.Add(pay);
						i += 1;
					}

					td = td.AddMonths(2);

				};
				return allpayment;
			}

			public List<Paymentitem> GetPastreccurance(Payment payment)
			{
				DateTime dt = DateTime.Now;
				DateTime td = payment.Data.RecurringPayment.StartDate;
				List<Paymentitem> allpayment = new List<Paymentitem>();
				while (DateTime.Compare(dt, td) >= 0)
				{
					payment.Data.Date = td;
					payment.Data.ChargedAccount = payment.Data.RecurringPayment.ChargedAccount;
					payment.Data.Amount = payment.Data.RecurringPayment.Amount;
					payment.Data.Type = payment.Data.RecurringPayment.Type;
					Paymentitem pay = new Paymentitem
					{
						dateTime = td,
						P = payment
					};
					allpayment.Add(pay);
					td = td.AddMonths(2);

				};
				return allpayment;
			}
		}

		public class RecurrQuarterly : iReccurance
		{
			public List<Paymentitem> GetFutureReccurance(Payment payment)
			{
				int i = 1;
				DateTime dt = DateTime.Now;
				DateTime td = payment.Data.RecurringPayment.StartDate;
				List<Paymentitem> allpayment = new List<Paymentitem>();
				while (i < 9)
				{
					payment.Data.Date = td;
					payment.Data.ChargedAccount = payment.Data.RecurringPayment.ChargedAccount;
					payment.Data.Amount = payment.Data.RecurringPayment.Amount;
					payment.Data.Type = payment.Data.RecurringPayment.Type;
					if (DateTime.Compare(dt, td) <= 0)
					{
						Paymentitem pay = new Paymentitem
						{
							dateTime = td,
							P = payment
						};
						allpayment.Add(pay);
						i += 1;
					}

					td = td.AddMonths(4);

				};
				return allpayment;
			}

			public List<Paymentitem> GetPastreccurance(Payment payment)
			{

				DateTime dt = DateTime.Now;
				DateTime td = payment.Data.RecurringPayment.StartDate;
				List<Paymentitem> allpayment = new List<Paymentitem>();
				while (DateTime.Compare(dt, td) >= 0)
				{
					payment.Data.Date = td;
					payment.Data.ChargedAccount = payment.Data.RecurringPayment.ChargedAccount;
					payment.Data.Amount = payment.Data.RecurringPayment.Amount;
					payment.Data.Type = payment.Data.RecurringPayment.Type;
					Paymentitem pay = new Paymentitem
					{
						dateTime = td,
						P = payment
					};
					allpayment.Add(pay);
					td = td.AddMonths(4);

				};
				return allpayment;
			}

		}

		public class RecurrYearly : iReccurance
		{
			public List<Paymentitem> GetFutureReccurance(Payment payment)
			{
				int i = 1;
				DateTime dt = DateTime.Now;
				DateTime td = payment.Data.RecurringPayment.StartDate;
				List<Paymentitem> allpayment = new List<Paymentitem>();
				while (i < 9)
				{
					payment.Data.Date = td;
					payment.Data.ChargedAccount = payment.Data.RecurringPayment.ChargedAccount;
					payment.Data.Amount = payment.Data.RecurringPayment.Amount;
					payment.Data.Type = payment.Data.RecurringPayment.Type;
					if (DateTime.Compare(dt, td) <= 0)
					{
						Paymentitem pay = new Paymentitem
						{
							dateTime = td,
							P = payment
						};
						allpayment.Add(pay);
						i += 1;
					}

					td = td.AddMonths(12);

				};
				return allpayment;
			}

			public List<Paymentitem> GetPastreccurance(Payment payment)
			{

				DateTime dt = DateTime.Now;
				DateTime td = payment.Data.RecurringPayment.StartDate;
				List<Paymentitem> allpayment = new List<Paymentitem>();
				while (DateTime.Compare(dt, td) >= 0)
				{
					payment.Data.Date = td;
					payment.Data.ChargedAccount = payment.Data.RecurringPayment.ChargedAccount;
					payment.Data.Amount = payment.Data.RecurringPayment.Amount;
					payment.Data.Type = payment.Data.RecurringPayment.Type;
					Paymentitem pay = new Paymentitem
					{
						dateTime = td,
						P = payment
					};
					allpayment.Add(pay);
					td = td.AddMonths(12);

				};
				return allpayment;
			}

		}

		public	class RecurrbiYearly : iReccurance
		{
			public List<Paymentitem> GetFutureReccurance(Payment payment)
			{
				int i = 1;
				DateTime dt = DateTime.Now;
				DateTime td = payment.Data.RecurringPayment.StartDate;
				List<Paymentitem> allpayment = new List<Paymentitem>();
				while (i < 9)
				{
					payment.Data.Date = td;
					payment.Data.ChargedAccount = payment.Data.RecurringPayment.ChargedAccount;
					payment.Data.Amount = payment.Data.RecurringPayment.Amount;
					payment.Data.Type = payment.Data.RecurringPayment.Type;
					if (DateTime.Compare(dt, td) <= 0)
					{
						Paymentitem pay = new Paymentitem
						{
							dateTime = td,
							P = payment
						};
						allpayment.Add(pay);
						i += 1;
					}

					td = td.AddMonths(24);

				};
				return allpayment;
			}

			public List<Paymentitem> GetPastreccurance(Payment payment)
			{

				DateTime dt = DateTime.Now;
				DateTime td = payment.Data.RecurringPayment.StartDate;
				List<Paymentitem> allpayment = new List<Paymentitem>();
				while (DateTime.Compare(dt, td) >= 0)
				{
					payment.Data.Date = td;
					payment.Data.ChargedAccount = payment.Data.RecurringPayment.ChargedAccount;
					payment.Data.Amount = payment.Data.RecurringPayment.Amount;
					payment.Data.Type = payment.Data.RecurringPayment.Type;
					Paymentitem pay = new Paymentitem
					{
						dateTime = td,
						P = payment
					};
					allpayment.Add(pay);
					td = td.AddMonths(24);

				};
				return allpayment;
			}
		}
		public struct Paymentitem
		{
			public DateTime dateTime;
			public Payment P;
		}
	}
}
