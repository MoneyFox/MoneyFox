﻿using MoneyFox.Application.Common.Interfaces;
using StoreKit;

namespace MoneyFox.iOS
{
    /// <inheritdoc />
    public class StoreOperations : IStoreOperations
    {
        /// <inheritdoc />
        public void RateApp() => SKStoreReviewController.RequestReview();
    }
}