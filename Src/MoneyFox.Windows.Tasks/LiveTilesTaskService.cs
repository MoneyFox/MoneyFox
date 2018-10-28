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
		Dictionary<DateTime, Payment> allpayment = new Dictionary<DateTime, Payment>();
		List<DateTime> alldates = new List<DateTime>();
		ApplicationDataContainer appsettings = ApplicationData.Current.LocalSettings;
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
				var next = await Get9NextpaymentsAsync();
				var previous = await Get9PreviouspaymentsAsync();
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
			var tiles =await SecondaryTile.FindAllForPackageAsync();
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
													Text = "Account: " + item.TileId,
													HintStyle = AdaptiveTextStyle.Caption
												},
											new AdaptiveText()
												{
													Text = "Balance: " +await GetLatestBalanceAsync(int.Parse(item.TileId)),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
												new AdaptiveText()
												{
													Text = "Last Month Payment Expenses: " + await GetMonthExpensesAsync(DateTime.Now.AddMonths(-1).Month,int.Parse(item.TileId)),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
											   new AdaptiveText()
												{
													Text = "This Month Payment Expenses: " + await  GetMonthExpensesAsync(DateTime.Now.Month,int.Parse(item.TileId)),
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
													Text = "Account: " + item.TileId,
													HintStyle = AdaptiveTextStyle.Caption
												},
											new AdaptiveText()
												{
													Text = "Balance: " +await GetLatestBalanceAsync(int.Parse(item.TileId)),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
												new AdaptiveText()
												{
													Text = "Last Month Payment Expenses: " + await GetMonthExpensesAsync(DateTime.Now.AddMonths(-1).Month,int.Parse(item.TileId)),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
											   new AdaptiveText()
												{
													Text = "This Month Payment Expenses: " + await  GetMonthExpensesAsync(DateTime.Now.Month,int.Parse(item.TileId)),
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
													Text = "Account: " + item.TileId,
													HintStyle = AdaptiveTextStyle.Caption
												},
											new AdaptiveText()
												{
													Text = "Balance: " +await GetLatestBalanceAsync(int.Parse(item.TileId)),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
												new AdaptiveText()
												{
													Text = "Last Month Payment Expenses: " + await GetMonthExpensesAsync(DateTime.Now.AddMonths(-1).Month,int.Parse(item.TileId)),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
											   new AdaptiveText()
												{
													Text = "This Month Payment Expenses: " + await  GetMonthExpensesAsync(DateTime.Now.Month,int.Parse(item.TileId)),
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
													Text = "Account: " + accountid.ToString(),
													HintStyle = AdaptiveTextStyle.Caption
												},
											new AdaptiveText()
												{
													Text = "Balance: " +await GetLatestBalanceAsync(accountid),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
												new AdaptiveText()
												{
													Text = "Last Month Payment Expenses: " + await GetMonthExpensesAsync(DateTime.Now.AddMonths(-1).Month,accountid),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
											   new AdaptiveText()
												{
													Text = "This Month Payment Expenses: " + await  GetMonthExpensesAsync(DateTime.Now.Month,accountid),
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
													Text = "Account: " + accountid.ToString(),
													HintStyle = AdaptiveTextStyle.Caption
												},
											new AdaptiveText()
												{
													Text = "Balance: " +await GetLatestBalanceAsync(accountid),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
												new AdaptiveText()
												{
													Text = "Last Month Payment Expenses: " + await GetMonthExpensesAsync(DateTime.Now.AddMonths(-1).Month,accountid),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
											   new AdaptiveText()
												{
													Text = "This Month Payment Expenses: " + await  GetMonthExpensesAsync(DateTime.Now.Month,accountid),
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
													Text = "Account: " + accountid.ToString(),
													HintStyle = AdaptiveTextStyle.Caption
												},
											new AdaptiveText()
												{
													Text = "Balance: " +await GetLatestBalanceAsync(accountid),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
												new AdaptiveText()
												{
													Text = "Last Month Payment Expenses: " + await GetMonthExpensesAsync(DateTime.Now.AddMonths(-1).Month,accountid),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
											   new AdaptiveText()
												{
													Text = "This Month Payment Expenses: " + await  GetMonthExpensesAsync(DateTime.Now.Month,accountid),
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
			double balance = 0.00;
			Account account = await accountService.GetById(accountid);
			var ticks = DateTime.Now.Ticks;
			if (account != null)
			{

				List<PaymentEntity> payments = account.Data.ChargedPayments.FindAll(x => x.Date.Month == month);
				foreach (PaymentEntity item in payments)
				{
					if (item.Type == Foundation.PaymentType.Expense)
					{
						balance += item.Amount;
					}

				}
				List<RecurringPaymentEntity> recurringPaymentEntities = account.Data.ChargedRecurringPayments.FindAll(x => x?.EndDate?.Month >= month && x.StartDate.Month <= month);
				foreach (RecurringPaymentEntity recurring in recurringPaymentEntities)
				{
					if (recurring.Type == Foundation.PaymentType.Expense)
					{
						balance += recurring.Amount;

					}
				}
				return balance;

			}
			else
			{
				return balance;
			}

		}
		private async Task<List<string>> Get9PreviouspaymentsAsync()
		{
			var acct = await accountService.GetAllAccounts();
			DateTime today = DateTime.Now;
			List<Payment> allpayments = new List<Payment>();
			List<PaymentEntity> newlst = new List<PaymentEntity>();
			List<RecurringPaymentEntity> allrecurring = new List<RecurringPaymentEntity>();
			foreach (Account item in acct)
			{
				allpayments.AddRange(await paymentService.GetPaymentsByAccountId(item.Data.Id));
			}
			foreach (Payment item in allpayments)
			{
				if (item.Data.IsRecurring)
				{
					Getrecurancepayments(item, item.Data.RecurringPayment.Recurrence, "last");
				}
				else
				{
					allpayment.Add(item.Data.Date, item);
				}
			}
			var tet = alldates.OrderBy(x => x.Date).TakeLast(8);
			List<string> returnlist = new List<string>();
			foreach (var item in tet)
			{
				Payment p = allpayment[item];
				returnlist.Add($"Payment {p.Data.Amount} paid on {p.Data.Date}");
			}
			alldates.Clear();
			allpayment.Clear();
			return returnlist;

		}
		private async Task<List<string>> Get9NextpaymentsAsync()
		{
			var acct = await accountService.GetAllAccounts();
			DateTime today = DateTime.Now;
			List<Payment> allpayments = new List<Payment>();
			List<PaymentEntity> newlst = new List<PaymentEntity>();
			List<RecurringPaymentEntity> allrecurring = new List<RecurringPaymentEntity>();
			foreach (Account item in acct)
			{
				allpayments.AddRange(await paymentService.GetPaymentsByAccountId(item.Data.Id));
			}
			foreach (Payment item in allpayments)
			{
				if (item.Data.IsRecurring)
				{
					item.Data.Amount = item.Data.RecurringPayment.Amount;

				}
				else
				{
					newlst.Add(item.Data);
				}
			}
			List<Payment> sortedlist = new List<Payment>();
			var te = newlst.OrderBy(x => DateTime.Compare(today, x.Date) <= 0).Take(9);
			var tes = allrecurring.OrderBy(x => DateTime.Compare(today, x.StartDate) <= 0 && DateTime.Compare(today, DateTime.Parse(x.EndDate.HasValue ? x.EndDate.ToString() : null)) <= 0).Take(9);
			List<PaymentEntity> test = (List<PaymentEntity>)te;
			foreach (var item in tes)
			{
				PaymentEntity payments = new PaymentEntity();
				payments.RecurringPayment = item;
				payments.Date = DateTime.Parse(DateTime.Now.Month + "/" + item.StartDate.Day + "/" + DateTime.Now.Year);
				payments.Amount = item.Amount;
				test.Add(payments);
			}
			var tested = test.OrderBy(x => x.Date);
			var tet = tested.Take(9);
			List<string> returnlist = new List<string>();
			foreach (var item in tet)
			{
				returnlist.Add($"Payment " + item.Amount + " paid on " + item.Date);
			}

			return returnlist;

		}
		private async Task<double> GetLatestBalanceAsync(int tileid)
		{
			var te = await accountService.GetById(tileid);
			return te.Data.CurrentBalance;
		}
		private void Getrecurancepayments(Payment p, Foundation.PaymentRecurrence reccur, string pastorfuture)
		{
			DateTime dt = DateTime.Now;
			switch (pastorfuture)
			{
				case "last":
					switch (reccur)
					{
						case Foundation.PaymentRecurrence.Daily:
							{
								DateTime td = p.Data.RecurringPayment.StartDate;
								while (DateTime.Compare(dt, td) >= 0)
								{
									p.Data.Date = td;
									p.Data.Amount = p.Data.RecurringPayment.Amount;
									allpayment.Add(td, p);
									alldates.Add(td);
									td = td.AddDays(1);

								};
								break;
							}
						case Foundation.PaymentRecurrence.DailyWithoutWeekend:
							{
								DateTime td = p.Data.RecurringPayment.StartDate;
								while (DateTime.Compare(dt, td) >= 0)
								{
									if (td.DayOfWeek == DayOfWeek.Saturday || td.DayOfWeek == DayOfWeek.Sunday)
									{
										continue;
									}
									else
									{

										p.Data.Date = td;
										p.Data.Amount = p.Data.RecurringPayment.Amount;

										allpayment.Add(td, p);
										alldates.Add(td);
										td = td.AddDays(1);
									}
								};
								break;
							}
						case Foundation.PaymentRecurrence.Weekly:
							{
								DateTime td = p.Data.RecurringPayment.StartDate;
								while (DateTime.Compare(dt, td) >= 0)
								{
									p.Data.Date = td;
									p.Data.Amount = p.Data.RecurringPayment.Amount;
									allpayment.Add(td, p);
									alldates.Add(td);
									td = td.AddDays(7);
								}
								break;
							}
						case Foundation.PaymentRecurrence.Biweekly:
							{
								DateTime td = p.Data.RecurringPayment.StartDate;
								while (DateTime.Compare(dt, td) >= 0)
								{
									p.Data.Date = td;
									p.Data.Amount = p.Data.RecurringPayment.Amount;
									allpayment.Add(td, p);
									alldates.Add(td);
									td = td.AddDays(14);
								};
								break;
							}
						case Foundation.PaymentRecurrence.Monthly:
							{
								DateTime td = p.Data.RecurringPayment.StartDate;
								while (DateTime.Compare(dt, td) >= 0)
								{
									p.Data.Date = td;
									p.Data.Amount = p.Data.RecurringPayment.Amount;
									allpayment.Add(td, p);
									alldates.Add(td);
									td = td.AddMonths(1);
								};
								break;
							}
						case Foundation.PaymentRecurrence.Bimonthly:
							{
								DateTime td = p.Data.RecurringPayment.StartDate;
								while (DateTime.Compare(dt, td) >= 0)
								{
									p.Data.Date = td;
									p.Data.Amount = p.Data.RecurringPayment.Amount;
									allpayment.Add(td, p);
									alldates.Add(td);
									td = td.AddMonths(2);
								};
								break;
							}
						case Foundation.PaymentRecurrence.Quarterly:
							{
								DateTime td = p.Data.RecurringPayment.StartDate;
								while (DateTime.Compare(dt, td) >= 0)
								{

									p.Data.Date = td;
									p.Data.Amount = p.Data.RecurringPayment.Amount; 
									allpayment.Add(td, p);
									alldates.Add(td);
									td = td.AddMonths(4);
								};
								break;
							}
						case Foundation.PaymentRecurrence.Biannually:
							{
								DateTime td = p.Data.RecurringPayment.StartDate;
								while (DateTime.Compare(dt, td) >= 0)
								{
									p.Data.Date = td;
									p.Data.Amount = p.Data.RecurringPayment.Amount;
									allpayment.Add(td, p);
									alldates.Add(td);
									td = td.AddMonths(6);
								};
								break;
							}
						case Foundation.PaymentRecurrence.Yearly:
							{
								DateTime td = p.Data.RecurringPayment.StartDate;
								while (DateTime.Compare(dt, td) >= 0)
								{
									p.Data.Date = td;
									p.Data.Amount = p.Data.RecurringPayment.Amount;
									allpayment.Add(td, p);
									alldates.Add(td);
									td = td.AddYears(1);
								};
								break;
							}
						default:
							break;
					}
					break;
				case "next":
					int i = 0;
					switch (reccur)
					{
						case Foundation.PaymentRecurrence.Daily:
							{

								DateTime td = p.Data.RecurringPayment.StartDate;
								while (DateTime.Compare(dt, td) > 0)
								{
									td = td.AddDays(1);
								};
								while (DateTime.Compare(dt, td) <= 0 && i <= 8)
								{
									p.Data.Date = td;
									p.Data.Amount = p.Data.RecurringPayment.Amount; 
									allpayment.Add(td, p);
									alldates.Add(td);
									td = td.AddDays(1);
									i += 1;
								}
								break;
							}
						case Foundation.PaymentRecurrence.DailyWithoutWeekend:
							{
								DateTime td = p.Data.RecurringPayment.StartDate;
								while (DateTime.Compare(dt, td) > 0)
								{
									td = td.AddDays(1);
								};
								while (DateTime.Compare(dt, td) <= 0 && i < 8)
								{
									if (td.DayOfWeek == DayOfWeek.Saturday || td.DayOfWeek == DayOfWeek.Sunday)
									{
										continue;
									}
									else
									{
										p.Data.Date = td;
										p.Data.Amount = p.Data.RecurringPayment.Amount;
										allpayment.Add(td, p);
										alldates.Add(td);
										td = td.AddDays(1);
										i += 1;
									}
								};
								break;
							}
						case Foundation.PaymentRecurrence.Weekly:
							{
								DateTime td = p.Data.RecurringPayment.StartDate;
								while (DateTime.Compare(dt, td) > 0)
								{

									td = td.AddDays(7);
								}
								while (DateTime.Compare(dt, td) <= 0 && i < 8)
								{
									p.Data.Date = td;
									p.Data.Amount = p.Data.RecurringPayment.Amount;
									allpayment.Add(td, p);
									alldates.Add(td);
									td = td.AddDays(7);
									i += 1;
								}
								break;
							}
						case Foundation.PaymentRecurrence.Biweekly:
							{
								DateTime td = p.Data.RecurringPayment.StartDate;
								while (DateTime.Compare(dt, td) > 0)
								{


									td = td.AddDays(14);
								};
								while (DateTime.Compare(dt, td) <= 0 && i < 8)
								{
									p.Data.Date = td;
									p.Data.Amount = p.Data.RecurringPayment.Amount;
									allpayment.Add(td, p);
									alldates.Add(td);
									td = td.AddDays(14);
									i += 1;
								};
								break;
							}
						case Foundation.PaymentRecurrence.Monthly:
							{
								DateTime td = p.Data.RecurringPayment.StartDate;
								while (DateTime.Compare(dt, td) >= 0)
								{
									td = td.AddMonths(1);
								};
								while (DateTime.Compare(dt, td) <= 0 && i < 8)
								{
									p.Data.Date = td;
									p.Data.Amount = p.Data.RecurringPayment.Amount;
									allpayment.Add(td, p);
									alldates.Add(td);
									td = td.AddMonths(1);
									i += 1;
								};
								break;
							}
						case Foundation.PaymentRecurrence.Bimonthly:
							{
								DateTime td = p.Data.RecurringPayment.StartDate;
								while (DateTime.Compare(dt, td) > 0)
								{
									td = td.AddMonths(2);
								};
								while (DateTime.Compare(dt, td) <= 0 && i < 8)
								{

									p.Data.Date = td;
									p.Data.Amount = p.Data.RecurringPayment.Amount; 
									allpayment.Add(td, p);
									alldates.Add(td);
									td = td.AddMonths(2);
								};
								break;
							}
						case Foundation.PaymentRecurrence.Quarterly:
							{
								DateTime td = p.Data.RecurringPayment.StartDate;
								while (DateTime.Compare(dt, td) > 0)
								{
									td = td.AddMonths(4);
								};
								while (DateTime.Compare(dt, td) <= 0 && i < 8)
								{
									p.Data.Date = td;
									p.Data.Amount = p.Data.RecurringPayment.Amount;
									allpayment.Add(td, p);
									alldates.Add(td);
									td = td.AddMonths(4);
									i += 1;
								};
								break;
							}
						case Foundation.PaymentRecurrence.Biannually:
							{
								DateTime td = p.Data.RecurringPayment.StartDate;
								while (DateTime.Compare(dt, td) > 0)
								{

									td = td.AddMonths(6);
								};
								while (DateTime.Compare(dt, td) <= 0 && i < 8)
								{
									p.Data.Date = td;
									p.Data.Amount = p.Data.RecurringPayment.Amount;
									allpayment.Add(td, p);
									alldates.Add(td);
									td = td.AddMonths(6);
									i += 1;
								};
								break;
							}
						case Foundation.PaymentRecurrence.Yearly:
							{
								DateTime td = p.Data.RecurringPayment.StartDate;
								while (DateTime.Compare(dt, td) > 0)
								{

									td = td.AddYears(1);
								};
								while (DateTime.Compare(dt, td) <= 0 && i < 8)
								{
									p.Data.Date = td;
									p.Data.Amount = p.Data.RecurringPayment.Amount;
									allpayment.Add(td, p);
									alldates.Add(td);
									td = td.AddYears(1);
								};
								break;
							}
						default:
							break;
					}
					break;
				default:
					break;
			}
		}
	}
}

