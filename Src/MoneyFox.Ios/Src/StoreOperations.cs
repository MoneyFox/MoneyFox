namespace MoneyFox.iOS
{
    using Core._Pending_.Common.Interfaces;
    using StoreKit;

    /// <inheritdoc />
    public class StoreOperations : IStoreOperations
    {
        /// <inheritdoc />
        public void RateApp() => SKStoreReviewController.RequestReview();
    }
}