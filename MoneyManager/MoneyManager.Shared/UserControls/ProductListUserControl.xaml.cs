using System;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Store;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using BugSense;
using MoneyManager.Business.Logic;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;

namespace MoneyManager.UserControls
{
    public sealed partial class ProductListUserControl
    {
        public ProductListUserControl()
        {
            InitializeComponent();

            RenderStoreItems();
        }

        public ObservableCollection<ProductItem> picItems = new ObservableCollection<ProductItem>();

        private async void RenderStoreItems()
        {
            picItems.Clear();

            try
            {
                var li = await CurrentApp.LoadListingInformationAsync();

                foreach (string key in li.ProductListings.Keys)
                {
                    ProductListing pListing = li.ProductListings[key];
                    var status = CurrentApp.LicenseInformation.ProductLicenses[key].IsActive ? Translation.GetTranslation("PurchasedLabel") : pListing.FormattedPrice;

                    picItems.Add(
                        new ProductItem
                        {
                            imgLink = key.Equals("10001") ? "/Images/{0}/UnlockFeatures.png" : "/Images/add.png.png",
                            Name = pListing.Name,
                            Status = status,
                            Key = key,
                            Description = pListing.Description,
                            BuyNowButtonVisible = CurrentApp.LicenseInformation.ProductLicenses[key].IsActive ? Visibility.Collapsed : Visibility.Visible
                        }
                    );
                }

                Plugin.ItemsSource = picItems;
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(Translation.GetTranslation("ProductNotFoundMessage"),
                    Translation.GetTranslation("ProductNotFoundTitle"));
                dialog.Commands.Add(new UICommand(Translation.GetTranslation("Ok")));

                BugSenseHandler.Instance.LogException(ex);
            }
        }

        private async void ButtonBuyNow_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;

                var key = btn.Tag.ToString();

                if (!CurrentApp.LicenseInformation.ProductLicenses[key].IsActive)
                {
                    var products = await CurrentApp.LoadListingInformationAsync();

                    ProductListing productListing;
                    if (!products.ProductListings.TryGetValue(LicenseHelper.FeaturepackProductKey, out productListing))
                    {
                        var dialog = new MessageDialog(Translation.GetTranslation("ProductNotFoundMessage"),
                            Translation.GetTranslation("ProductNotFoundTitle"));
                        dialog.Commands.Add(new UICommand(Translation.GetTranslation("Ok")));
                        return;
                    }

                    await CurrentApp.RequestProductPurchaseAsync(productListing.ProductId);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("0x80004005"))
                {
                    var dialog = new MessageDialog(Translation.GetTranslation("PurchasedFailedMessage"),
                        Translation.GetTranslation("PurchasedFailedTitle"));
                    dialog.Commands.Add(new UICommand(Translation.GetTranslation("Ok")));
                }
                else
                {
                    BugSenseHandler.Instance.LogException(ex);
                }
            }
        }

    }
}
