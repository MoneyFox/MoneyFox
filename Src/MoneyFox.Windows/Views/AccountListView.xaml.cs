using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using MoneyFox.Business.ViewModels;
using MoneyFox.Business.ViewModels.DesignTime;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using System;
using Windows.UI.StartScreen;
using Microsoft.Toolkit.Uwp.Notifications;
using MoneyFox.Windows.Business;
using Windows.UI.Notifications;

namespace MoneyFox.Windows.Views
{
	/// <summary>
	///     View to display an list of accounts.
	/// </summary>
	public sealed partial class AccountListView
	{
		/// <summary>
		///     Initialize View.
		/// </summary>
		public AccountListView()
		{
			InitializeComponent();

			if (DesignMode.DesignModeEnabled)
			{
				ViewModel = new DesignTimeAccountListViewModel();
			}
		}

		private void AccountList_RightTapped(object sender, RightTappedRoutedEventArgs e)
		{
			var senderElement = sender as FrameworkElement;
			var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement) as MenuFlyout;

			flyoutBase?.ShowAt(senderElement, e.GetPosition(senderElement));
		}

		private void Edit_OnClick(object sender, RoutedEventArgs e)
		{
			var element = (FrameworkElement)sender;
			var account = element.DataContext as AccountViewModel;
			if (account == null)
			{
				return;
			}

			(DataContext as AccountListViewModel)?.EditAccountCommand.Execute(account);
		}

		private void Delete_OnClick(object sender, RoutedEventArgs e)
		{
			//this has to be called before the dialog service since otherwise the datacontext is reseted and the account will be null
			var element = (FrameworkElement)sender;
			var account = element.DataContext as AccountViewModel;
			if (account == null)
			{
				return;
			}

			(DataContext as AccountListViewModel)?.DeleteAccountCommand.Execute(account);
		}
		private AppServiceConnection liveTileService;
		private async void AddToStartMenu_ClickAsync(object sender, RoutedEventArgs e)
		{
			var element = (FrameworkElement)sender;
			var account = element.DataContext as AccountViewModel;
			if (account == null)
			{
				return;
			}
			var name = account.Account;
			int id = name.Data.Id;
			bool isPinned = SecondaryTile.Exists(id.ToString());
			if (!isPinned)
			{
			
				SecondaryTile tile = new SecondaryTile(id.ToString(),"Money Fox","Home",new Uri("ms-appx:///Assets/SmallTile.scale-150.png"),TileSize.Default);
				bool ispinned = await tile.RequestCreateAsync();
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
													Text = string.Format(CommonFunctions.GetResourceKey("LiveTileAccountName"),id.ToString()),
													HintStyle = AdaptiveTextStyle.Caption
												},
											new AdaptiveText()
												{
													Text = string.Format(CommonFunctions.GetResourceKey("LiveTileAccountBalance"),await CommonFunctions.GetLatestBalanceAsync(id)),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
												new AdaptiveText()
												{
													Text = string.Format(CommonFunctions.GetResourceKey("LiveTileAccountLastMonthsExpenses"),await CommonFunctions.GetMonthExpensesAsync(DateTime.Now.AddMonths(-1).Month,id)),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
											   new AdaptiveText()
												{
													Text = string.Format(CommonFunctions.GetResourceKey("LiveTileAccountCurrentMonthsExpenses"), await CommonFunctions.GetMonthExpensesAsync(DateTime.Now.Month,id)),
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
													Text = string.Format(CommonFunctions.GetResourceKey("LiveTileAccountName"),id.ToString()),
													HintStyle = AdaptiveTextStyle.Caption
												},
											new AdaptiveText()
												{
													Text = string.Format(CommonFunctions.GetResourceKey("LiveTileAccountBalance"),await CommonFunctions.GetLatestBalanceAsync(id)),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
												new AdaptiveText()
												{
													Text = string.Format(CommonFunctions.GetResourceKey("LiveTileAccountLastMonthsExpenses"),await CommonFunctions.GetMonthExpensesAsync(DateTime.Now.AddMonths(-1).Month,id)),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
											   new AdaptiveText()
												{
													Text = string.Format(CommonFunctions.GetResourceKey("LiveTileAccountCurrentMonthsExpenses"), await CommonFunctions.GetMonthExpensesAsync(DateTime.Now.Month,id)),
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
													Text = string.Format(CommonFunctions.GetResourceKey("LiveTileAccountName"),id.ToString()),
													HintStyle = AdaptiveTextStyle.Caption
												},
											new AdaptiveText()
												{
													Text = string.Format(CommonFunctions.GetResourceKey("LiveTileAccountBalance"),await CommonFunctions.GetLatestBalanceAsync(id)),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
												new AdaptiveText()
												{
													Text = string.Format(CommonFunctions.GetResourceKey("LiveTileAccountLastMonthsExpenses"),await CommonFunctions.GetMonthExpensesAsync(DateTime.Now.AddMonths(-1).Month,id)),
													HintStyle = AdaptiveTextStyle.CaptionSubtle
												},
											   new AdaptiveText()
												{
													Text = string.Format(CommonFunctions.GetResourceKey("LiveTileAccountCurrentMonthsExpenses"), await CommonFunctions.GetMonthExpensesAsync(DateTime.Now.Month,id)),
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
					TileUpdateManager.CreateTileUpdaterForSecondaryTile(id.ToString()).Update(tn);
				}
			}
		}
	}
}