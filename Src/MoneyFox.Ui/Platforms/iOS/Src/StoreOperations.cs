namespace MoneyFox.Ui.Platforms.iOS.Src;

using Core.Common.Interfaces;
using StoreKit;

public class StoreOperations : IStoreOperations
{
    public void RateApp()
    {
        SKStoreReviewController.RequestReview();
    }
}
