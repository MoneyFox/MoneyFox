namespace MoneyFox.Ui.Platforms.iOS.Src
{

    using Core.Common.Interfaces;
    using StoreKit;

    /// <inheritdoc />
    public class StoreOperations : IStoreOperations
    {
        /// <inheritdoc />
        public void RateApp()
        {
            SKStoreReviewController.RequestReview();
        }
    }

}
