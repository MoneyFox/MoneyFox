using EntityFramework.DbContextScope;
using Microsoft.Toolkit.Uwp.Notifications;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Core;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using Windows.Storage;
using MoneyFox.Foundation.Resources;

namespace MoneyFox.Windows.Tasks
{
	/// <summary>
	/// Service and background task to create, and update live tiles	
	/// </summary>
	public sealed class UpdateliveandLockscreenTiles : IBackgroundTask
	{
		BackgroundTaskDeferral serviceDeferral;
		AppServiceConnection connection;
		AccountService accountService = new AccountService(new AmbientDbContextLocator(), new DbContextScopeFactory());
		PaymentService paymentService = new PaymentService(new AmbientDbContextLocator(), new DbContextScopeFactory());
		public List<Recurpaymentitem> allpayment = new List<Recurpaymentitem>();
		ApplicationDataContainer appsettings = ApplicationData.Current.LocalSettings;
		Dictionary<Foundation.PaymentRecurrence, Func<iReccurance>> strategy = new Dictionary<Foundation.PaymentRecurrence, Func<iReccurance>>();
		public void Run(IBackgroundTaskInstance taskInstance)
		{

			serviceDeferral = taskInstance.GetDeferral();
			taskInstance.Canceled += OnTaskCanceled;

			var details = taskInstance.TriggerDetails as AppServiceTriggerDetails;
			connection = details.AppServiceConnection;
			connection.RequestReceived += OnRequestReceivedAsync;
			serviceDeferral?.Complete();

		}
		private async void OnRequestReceivedAsync(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
		{
			var messageDeferral = args.GetDeferral();
			strategy.Add(Foundation.PaymentRecurrence.Daily, () => new RecurrDaily());
			strategy.Add(Foundation.PaymentRecurrence.DailyWithoutWeekend, () => new RecurrWeekdays());
			strategy.Add(Foundation.PaymentRecurrence.Weekly, () => new RecurrWeek());
			strategy.Add(Foundation.PaymentRecurrence.Biweekly, () => new RecurrBiWeekly());
			strategy.Add(Foundation.PaymentRecurrence.Monthly, () => new RecurrMonthly());
			strategy.Add(Foundation.PaymentRecurrence.Bimonthly, () => new RecurrBiMonthly());
			strategy.Add(Foundation.PaymentRecurrence.Quarterly, () => new RecurrQuarterly());
			strategy.Add(Foundation.PaymentRecurrence.Yearly, () => new RecurrYearly());
			strategy.Add(Foundation.PaymentRecurrence.Biannually, () => new RecurrbiYearly());
			try
			{
				var input = args.Request.Message;
				if (input != null)
				{
					if ((string)input["action"] == "create")
					{
						await CreateSecondaryLiveTile((int)input["accountid"]);

					}
					else
					{
						await UpdatePrimaryLiveTile();
						await UpdateSecondaryLiveTiles();
					}
				}
				else
				{
					await UpdatePrimaryLiveTile();
					await UpdateSecondaryLiveTiles();
				}
			}
			finally
			{

				messageDeferral.Complete();
			}
		}


		private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
		{
			serviceDeferral?.Complete();
			serviceDeferral = null;
		}

		private async Task UpdatePrimaryLiveTile()
		{
			AppListEntry entry = (await Package.Current.GetAppListEntriesAsync())[0];
			bool isPinned = await StartScreenManager.GetDefault().ContainsAppListEntryAsync(entry);
			if (isPinned)
			{
				object b = appsettings.Values["lastrun"];
				string lastrun = (string)b;
				string nextpayment = "Upcomming payments";
				string previouspayment = "Past payments";
				if (lastrun == "last")
				{
					appsettings.Values["lastrun"] = "next";
				
				}
				else
				{
					appsettings.Values["lastrun"] = "last";

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

		private async Task UpdateSecondaryLiveTiles()
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
													Text = string.Format(GetResourceString("LiveTileAccountName"),item.TileId),
													HintStyle = AdaptiveTextStyle.Caption
												},
											new AdaptiveText()
												{
													Text = string.Format(GetResourceString("LiveTileAccountBalance"),await GetLatestBalanceAsync(int.Parse(item.TileId))),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
												new AdaptiveText()
												{
													Text = string.Format(GetResourceString("LiveTileAccountLastMonthsExpenses"),await GetMonthExpensesAsync(DateTime.Now.AddMonths(-1).Month,int.Parse(item.TileId))),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
											   new AdaptiveText()
												{
													Text = string.Format(GetResourceString("LiveTileAccountCurrentMonthsExpenses"), await  GetMonthExpensesAsync(DateTime.Now.Month,int.Parse(item.TileId))),
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
													Text = string.Format(GetResourceString("LiveTileAccountName"),item.TileId),
													HintStyle = AdaptiveTextStyle.Caption
												},
											new AdaptiveText()
												{
													Text = string.Format(GetResourceString("LiveTileAccountBalance"),await GetLatestBalanceAsync(int.Parse(item.TileId))),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
												new AdaptiveText()
												{
													Text = string.Format(GetResourceString("LiveTileAccountLastMonthsExpenses"),await GetMonthExpensesAsync(DateTime.Now.AddMonths(-1).Month,int.Parse(item.TileId))),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
											   new AdaptiveText()
												{
													Text = string.Format(GetResourceString("LiveTileAccountCurrentMonthsExpenses"), await  GetMonthExpensesAsync(DateTime.Now.Month,int.Parse(item.TileId))),
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
													Text = string.Format(GetResourceString("LiveTileAccountName"),item.TileId),
													HintStyle = AdaptiveTextStyle.Caption
												},
											new AdaptiveText()
												{
													Text = string.Format(GetResourceString("LiveTileAccountBalance"),await GetLatestBalanceAsync(int.Parse(item.TileId))),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
												new AdaptiveText()
												{
													Text = string.Format(GetResourceString("LiveTileAccountLastMonthsExpenses"),await GetMonthExpensesAsync(DateTime.Now.AddMonths(-1).Month,int.Parse(item.TileId))),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
											   new AdaptiveText()
												{
													Text = string.Format(GetResourceString("LiveTileAccountCurrentMonthsExpenses"), await  GetMonthExpensesAsync(DateTime.Now.Month,int.Parse(item.TileId))),
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

		private async Task CreateSecondaryLiveTile(int accountid)
		{
			bool isPinned = SecondaryTile.Exists(accountid.ToString());
			if (!isPinned)
			{
				SecondaryTile tile = new SecondaryTile();
				tile.TileId = accountid.ToString();
				var ispinned = await tile.RequestCreateAsync();
				if (ispinned)
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
													Text = string.Format(GetResourceString("LiveTileAccountName"),accountid.ToString()),
													HintStyle = AdaptiveTextStyle.Caption
												},
											new AdaptiveText()
												{
													Text = string.Format(GetResourceString("LiveTileAccountBalance"),await GetLatestBalanceAsync(accountid)),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
												new AdaptiveText()
												{
													Text = string.Format(GetResourceString("LiveTileAccountLastMonthsExpenses"),await GetMonthExpensesAsync(DateTime.Now.AddMonths(-1).Month,accountid)),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
											   new AdaptiveText()
												{
													Text = string.Format(GetResourceString("LiveTileAccountCurrentMonthsExpenses"), await  GetMonthExpensesAsync(DateTime.Now.Month,accountid)),
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
													Text = string.Format(GetResourceString("LiveTileAccountName"),accountid.ToString()),
													HintStyle = AdaptiveTextStyle.Caption
												},
											new AdaptiveText()
												{
													Text = string.Format(GetResourceString("LiveTileAccountBalance"),await GetLatestBalanceAsync(accountid)),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
												new AdaptiveText()
												{
													Text = string.Format(GetResourceString("LiveTileAccountLastMonthsExpenses"),await GetMonthExpensesAsync(DateTime.Now.AddMonths(-1).Month,accountid)),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
											   new AdaptiveText()
												{
													Text = string.Format(GetResourceString("LiveTileAccountCurrentMonthsExpenses"), await  GetMonthExpensesAsync(DateTime.Now.Month,accountid)),
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
													Text = string.Format(GetResourceString("LiveTileAccountName"),accountid.ToString()),
													HintStyle = AdaptiveTextStyle.Caption
												},
											new AdaptiveText()
												{
													Text = string.Format(GetResourceString("LiveTileAccountBalance"),await GetLatestBalanceAsync(accountid)),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
												new AdaptiveText()
												{
													Text = string.Format(GetResourceString("LiveTileAccountLastMonthsExpenses"),await GetMonthExpensesAsync(DateTime.Now.AddMonths(-1).Month,accountid)),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
											   new AdaptiveText()
												{
													Text = string.Format(GetResourceString("LiveTileAccountCurrentMonthsExpenses"), await  GetMonthExpensesAsync(DateTime.Now.Month,accountid)),
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
					TileUpdateManager.CreateTileUpdaterForSecondaryTile(accountid.ToString()).Update(tn);
				}
			}

		}

		private async Task<double> GetMonthExpensesAsync(int month, int accountid)
		{
			var currentmonth = DateTime.Now.Month;
			double balance = 0.00;
			IEnumerable<Payment> payments = await paymentService.GetPaymentsByAccountId(accountid);
			foreach (Payment item in payments)
			{
				if (item.Data.IsRecurring)
				{
					iReccurance reccurance = strategy[item.Data.RecurringPayment.Recurrence]();
					reccurance.P = item;
					reccurance.Prevorfuture = "previous";
					allpayment.AddRange(reccurance.Getreccurance());

				}
				else
				{
					
						Recurpaymentitem pay = new Recurpaymentitem
						{
							dateTime=item.Data.Date,
							P=item
				};
		}
			}
			IEnumerable<Recurpaymentitem> all = allpayment.Where(x => x.dateTime.Month == month && x.dateTime.Year == DateTime.Now.Year).ToList();
			foreach (Recurpaymentitem item in all)
			{
				balance += item.P.Data.Amount;
			}
			return balance;
		}

		private async Task<List<string>> GetPreviouspaymentsAsync()
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
					reccurance.P = item;
					reccurance.Prevorfuture = "previous";
					allpayment.AddRange(reccurance.Getreccurance());
				 	
				}
				else
				{
					Recurpaymentitem keyValuePair = new Recurpaymentitem
					{
						dateTime = item.Data.Date,
						P = item
					};
					allpayment.Add(keyValuePair);
				}
			}
			IEnumerable<Recurpaymentitem> tet = allpayment.OrderByDescending(x => x.dateTime).Take(8);
			List<string> returnlist = new List<string>();
			foreach (var item in tet)
			{
				Payment p = item.P;
				returnlist.Add(string.Format(GetResourceString("LiveTilePastPaymentText"), p.Data.Amount, p.Data.ChargedAccount.Name, p.Data.Date));
			}
			allpayment.Clear();
			return returnlist;

		}

		private async Task<List<string>> GetNextpaymentsAsync()
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
					reccurance.P = item;
					reccurance.Prevorfuture = "next";
					allpayment.AddRange(reccurance.Getreccurance());
				}
				else
				{
					Recurpaymentitem pay = new Recurpaymentitem
					{
					dateTime = item.Data.Date,
					P=item
					};
					allpayment.Add(pay);
				}
			}
			IEnumerable<Recurpaymentitem> tet = allpayment.OrderBy(x => x.dateTime).Take(8);
			List<string> returnlist = new List<string>();
			foreach (var item in tet)
			{
				Payment P = item.P;
				returnlist.Add(string.Format(GetResourceString("LiveTileFuturePaymentText"), P.Data.Amount, P.Data.ChargedAccount.Name, P.Data.Date));
			}
			allpayment.Clear();
			
 
			return returnlist;

		}

		private async Task<double> GetLatestBalanceAsync(int tileid)
		{
			var te = await accountService.GetById(tileid);
			return te.Data.CurrentBalance;
		}

	

		private static string GetResourceString(string resourcekey)
		{
			System.Resources.ResourceManager keyValuePairs = Strings.ResourceManager;
			return keyValuePairs.GetString(resourcekey);
		}

		public interface iReccurance
		{
			string Prevorfuture { get; set; }
			Payment P { get; set; }
			List<Recurpaymentitem> Getreccurance();
		}
		public class RecurrDaily : iReccurance
		{
			public string Prevorfuture { get; set; }
			public Payment P { get; set; }

			public List<Recurpaymentitem> Getreccurance()
			{
				int i = 0;
				DateTime dt = DateTime.Now;
				DateTime td = P.Data.RecurringPayment.StartDate;
				List<Recurpaymentitem> allpayment = new List<Recurpaymentitem>();
				if (Prevorfuture == "previous")
				{
					while (DateTime.Compare(dt, td) >= 0)
					{
						P.Data.Date = td;
						P.Data.ChargedAccount = P.Data.RecurringPayment.ChargedAccount;
						P.Data.Amount = P.Data.RecurringPayment.Amount;
						Recurpaymentitem pay = new Recurpaymentitem
						{
							dateTime = td,
							P = P
						};
						allpayment.Add(pay);
						td = td.AddDays(1);
					};
				}
				else
				{
					while (DateTime.Compare(dt, td) <= 0 && i < 9)
					{
						P.Data.Date = td;
						P.Data.ChargedAccount = P.Data.RecurringPayment.ChargedAccount;
						P.Data.Amount = P.Data.RecurringPayment.Amount;
						Recurpaymentitem pay = new Recurpaymentitem
						{
							dateTime = td,
							P = P
						};
						allpayment.Add(pay);
						td = td.AddDays(1);
						i += 1;
					};
				}
				return allpayment;
			}
		}
		public class RecurrWeekdays : iReccurance
		{
			public string Prevorfuture { get; set; }
			public Payment P { get; set; }

			public List<Recurpaymentitem> Getreccurance()
			{
				DateTime dt = DateTime.Now;
				DateTime td = P.Data.RecurringPayment.StartDate;
				List<Recurpaymentitem> allpayment = new List<Recurpaymentitem>();
				int i = 0;
				if (Prevorfuture == "previous")
				{
					while (DateTime.Compare(dt, td) >= 0)
					{
						P.Data.Date = td;
						P.Data.ChargedAccount = P.Data.RecurringPayment.ChargedAccount;
						P.Data.Amount = P.Data.RecurringPayment.Amount;
						if (P.Data.Date.DayOfWeek != DayOfWeek.Saturday && P.Data.Date.DayOfWeek !=DayOfWeek.Sunday)
						{
							Recurpaymentitem pay = new Recurpaymentitem
							{
								dateTime = td,
								P = P
							};
							allpayment.Add(pay);
							td = td.AddDays(1);
						}
					
						td = td.AddDays(1);

					};
				}
				else
				{
					while (DateTime.Compare(dt, td) <= 0 && i < 9)
					{
						P.Data.Date = td;
						P.Data.ChargedAccount = P.Data.RecurringPayment.ChargedAccount;
						P.Data.Amount = P.Data.RecurringPayment.Amount;
						if (P.Data.Date.DayOfWeek != DayOfWeek.Saturday && P.Data.Date.DayOfWeek != DayOfWeek.Sunday)
						{
							Recurpaymentitem pay = new Recurpaymentitem
							{
								dateTime = td,
								P=P
							};
							allpayment.Add(pay);
							td = td.AddDays(1);
							i += 1;
						}

						td = td.AddDays(1);
					
					};
				}
				return allpayment;
			}
		}
		public class RecurrWeek : iReccurance
		{
			public string Prevorfuture { get; set; }
			public Payment P { get; set; }

			public List<Recurpaymentitem> Getreccurance()
			{
				int i = 0;
				DateTime dt = DateTime.Now;
				DateTime td = P.Data.RecurringPayment.StartDate;
				List<Recurpaymentitem> allpayment = new List<Recurpaymentitem>();
				if (Prevorfuture == "previous")
				{
					while (DateTime.Compare(dt, td) >= 0)
					{
						P.Data.Date = td;
						P.Data.ChargedAccount = P.Data.RecurringPayment.ChargedAccount;
						P.Data.Amount = P.Data.RecurringPayment.Amount;
						Recurpaymentitem pay = new Recurpaymentitem
						{
							dateTime = td,
							P = P
						};
						allpayment.Add(pay);
						td = td.AddDays(7);

					};
				}
				else
				{
					while (DateTime.Compare(dt, td) <= 0 && i < 9)
					{
						P.Data.Date = td;
						P.Data.ChargedAccount = P.Data.RecurringPayment.ChargedAccount;
						P.Data.Amount = P.Data.RecurringPayment.Amount;
						Recurpaymentitem pay = new Recurpaymentitem
						{
							dateTime = td,
							P = P
						};
						allpayment.Add(pay);
						td = td.AddDays(7);
						i += 1;
					};
				}
				return allpayment;
			}
		}
		public class RecurrBiWeekly : iReccurance
		{
			public string Prevorfuture { get; set; }
			public Payment P { get; set; }

			public List<Recurpaymentitem> Getreccurance()
			{
				int i = 0;
				DateTime dt = DateTime.Now;
				DateTime td = P.Data.RecurringPayment.StartDate;
				List<Recurpaymentitem> allpayment = new List<Recurpaymentitem>();
				if (Prevorfuture == "previous")
				{
					while (DateTime.Compare(dt, td) >= 0)
					{
						P.Data.Date = td;
						P.Data.ChargedAccount = P.Data.RecurringPayment.ChargedAccount;
						P.Data.Amount = P.Data.RecurringPayment.Amount;
						Recurpaymentitem pay = new Recurpaymentitem
						{
							dateTime = td,
							P = P
						};
						allpayment.Add(pay);
						td = td.AddDays(14);

					};
				}
				else
				{
					while (DateTime.Compare(dt, td) <= 0 && i < 9)
					{
						P.Data.Date = td;
						P.Data.ChargedAccount = P.Data.RecurringPayment.ChargedAccount;
						P.Data.Amount = P.Data.RecurringPayment.Amount;
						Recurpaymentitem pay = new Recurpaymentitem
						{
							dateTime = td,
							P = P
						};
						allpayment.Add(pay);
						td = td.AddDays(14);
						i += 1;
					};
				}
				return allpayment;
			}
		}
		public class RecurrMonthly : iReccurance
		{
			public string Prevorfuture { get; set; }
			public Payment P { get; set; }

			public List<Recurpaymentitem> Getreccurance()
			{
				int i = 0;
				DateTime dt = DateTime.Now;
				DateTime td = P.Data.RecurringPayment.StartDate;
				List<Recurpaymentitem> allpayment = new List<Recurpaymentitem>();
				if (Prevorfuture == "previous")
				{
					while (DateTime.Compare(dt, td) >= 0)
					{
						P.Data.Date = td;
						P.Data.ChargedAccount = P.Data.RecurringPayment.ChargedAccount;
						P.Data.Amount = P.Data.RecurringPayment.Amount;
						Recurpaymentitem pay = new Recurpaymentitem
						{
							dateTime = td,
							P = P
						};
						allpayment.Add(pay);
						td = td.AddMonths(1);

					};
				}
				else
				{
					while (DateTime.Compare(dt, td) <= 0 && i < 9)
					{
						P.Data.Date = td;
						P.Data.ChargedAccount = P.Data.RecurringPayment.ChargedAccount;
						P.Data.Amount = P.Data.RecurringPayment.Amount;
						Recurpaymentitem pay = new Recurpaymentitem
						{
							dateTime = td,
							P = P
						};
						allpayment.Add(pay);
						td = td.AddMonths(1);
						i += 1;
					};
				}
				return allpayment;
			}
		}
		public class RecurrBiMonthly : iReccurance
		{
			public string Prevorfuture { get; set; }
			public Payment P { get; set; }

			public List<Recurpaymentitem> Getreccurance()
			{
				int i = 0;
				DateTime dt = DateTime.Now;
				DateTime td = P.Data.RecurringPayment.StartDate;
				List<Recurpaymentitem> allpayment = new List<Recurpaymentitem>();
				if (Prevorfuture == "previous")
				{
					while (DateTime.Compare(dt, td) >= 0)
					{
						P.Data.Date = td;
						P.Data.ChargedAccount = P.Data.RecurringPayment.ChargedAccount;
						P.Data.Amount = P.Data.RecurringPayment.Amount;
						Recurpaymentitem pay = new Recurpaymentitem
						{
							dateTime = td,
							P = P
						};
						allpayment.Add(pay);
						td = td.AddMonths(2);

					};
				}
				else
				{
					while (DateTime.Compare(dt, td) <= 0 && i < 9)
					{
						P.Data.Date = td;
						P.Data.ChargedAccount = P.Data.RecurringPayment.ChargedAccount;
						P.Data.Amount = P.Data.RecurringPayment.Amount;
						Recurpaymentitem pay = new Recurpaymentitem
						{
							dateTime = td,
							P = P
						};
						allpayment.Add(pay);
						td = td.AddMonths(2);
						i += 1;
					};
				}
				return allpayment;
			}
		}
		public class RecurrQuarterly : iReccurance
		{
			public string Prevorfuture { get; set; }
			public Payment P { get; set; }

			public List<Recurpaymentitem> Getreccurance()
			{
				int i = 0;
				DateTime dt = DateTime.Now;
				DateTime td = P.Data.RecurringPayment.StartDate;
				List<Recurpaymentitem> allpayment = new List<Recurpaymentitem>();
				if (Prevorfuture == "previous")
				{
					while (DateTime.Compare(dt, td) >= 0)
					{
						P.Data.Date = td;
						P.Data.ChargedAccount = P.Data.RecurringPayment.ChargedAccount;
						P.Data.Amount = P.Data.RecurringPayment.Amount;
						Recurpaymentitem pay = new Recurpaymentitem
						{
							dateTime = td,
							P = P
						};
						allpayment.Add(pay);
						td = td.AddMonths(4);

					};
				}
				else
				{
					while (DateTime.Compare(dt, td) <= 0 && i < 9)
					{
						P.Data.Date = td;
						P.Data.ChargedAccount = P.Data.RecurringPayment.ChargedAccount;
						P.Data.Amount = P.Data.RecurringPayment.Amount;
						 Recurpaymentitem pay = new Recurpaymentitem
						{
							dateTime = td,
							P = P
						}; 
						allpayment.Add(pay);
						td = td.AddMonths(4);
						i += 1;
					};
				}
				return allpayment;
			}
		}
		public class RecurrYearly : iReccurance
		{
			public string Prevorfuture { get; set; }
			public Payment P { get; set; }

			public List<Recurpaymentitem> Getreccurance()
			{
				int i = 0;
				DateTime dt = DateTime.Now;
				DateTime td = P.Data.RecurringPayment.StartDate;
				List<Recurpaymentitem> allpayment = new List<Recurpaymentitem>();
				if (Prevorfuture == "previous")
				{
					while (DateTime.Compare(dt, td) >= 0)
					{
						P.Data.Date = td;
						P.Data.ChargedAccount = P.Data.RecurringPayment.ChargedAccount;
						P.Data.Amount = P.Data.RecurringPayment.Amount;
						Recurpaymentitem pay = new Recurpaymentitem
						{
							dateTime = td,
							P = P
						};
						allpayment.Add(pay);
						td = td.AddMonths(12);

					};
				}
				else
				{
					while (DateTime.Compare(dt, td) <= 0 && i < 9)
					{
						P.Data.Date = td;
						P.Data.ChargedAccount = P.Data.RecurringPayment.ChargedAccount;
						P.Data.Amount = P.Data.RecurringPayment.Amount;
						Recurpaymentitem pay = new Recurpaymentitem
						{
							dateTime = td,
							P = P
						};
						allpayment.Add(pay);
						td = td.AddMonths(12);
						i += 1;
					};
				}
				return allpayment;
			}
		}
		public class RecurrbiYearly : iReccurance
		{
			public string Prevorfuture { get; set; }
			public Payment P { get; set; }

			public List<Recurpaymentitem> Getreccurance()
			{
				int i = 0;
				DateTime dt = DateTime.Now;
				DateTime td = P.Data.RecurringPayment.StartDate;
				List<Recurpaymentitem> allpayment = new List<Recurpaymentitem>();
				if (Prevorfuture == "previous")
				{
					while (DateTime.Compare(dt, td) >= 0)
					{
						P.Data.Date = td;
						P.Data.ChargedAccount = P.Data.RecurringPayment.ChargedAccount;
						P.Data.Amount = P.Data.RecurringPayment.Amount;
						Recurpaymentitem pay = new Recurpaymentitem
						{
							dateTime = td,
							P = P
						};
						allpayment.Add(pay);
						td = td.AddMonths(24);

					};
				}
				else
				{
					while (DateTime.Compare(dt, td) <= 0 && i < 9)
					{
						P.Data.Date = td;
						P.Data.ChargedAccount = P.Data.RecurringPayment.ChargedAccount;
						P.Data.Amount = P.Data.RecurringPayment.Amount;
						Recurpaymentitem pay = new Recurpaymentitem
						{
							dateTime = td,
							P = P
						};
						allpayment.Add(pay);
						td = td.AddMonths(24);
						i += 1;
					};
				}
				return allpayment;
			}
		}
	}
	public struct Recurpaymentitem
	{
		public DateTime dateTime;
		public Payment P;
	}
}