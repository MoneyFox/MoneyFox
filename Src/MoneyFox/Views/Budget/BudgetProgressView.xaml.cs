namespace MoneyFox.Views.Budget
{

    using System;
    using Xamarin.Forms;

    public partial class BudgetProgressView
    {
        public static readonly BindableProperty CurrentSpendingProperty = BindableProperty.Create(
            propertyName: "CurrentSpending",
            returnType: typeof(decimal),
            declaringType: typeof(BudgetProgressView),
            defaultValue: 0m);

        public static readonly BindableProperty SpendingLimitProperty = BindableProperty.Create(
            propertyName: "SpendingLimit",
            returnType: typeof(decimal),
            declaringType: typeof(BudgetProgressView),
            defaultValue: 0m);

        public BudgetProgressView()
        {
            InitializeComponent();
        }

        public decimal CurrentSpending
        {
            get
            {
                var currentSpending = (decimal)GetValue(CurrentSpendingProperty);
                UpdateCurrentSpendingBar(currentSpending);

                return currentSpending;
            }

            set => SetValue(property: CurrentSpendingProperty, value: value);
        }

        public decimal SpendingLimit
        {
            get => (decimal)GetValue(SpendingLimitProperty);
            set => SetValue(property: SpendingLimitProperty, value: value);
        }

        /// <summary>
        ///     This handler is necessary, so that the bar is correctly updated when the user scrolls in the list.
        ///     Otherwise the list on iOS would cache the progress bar.
        /// </summary>
        private void UpdateCurrentSpendingBar(decimal currentSpending)
        {
            if (currentSpending == 0 || SpendingLimit == 0)
            {
                CurrentSpendingBar.WidthRequest = 0;
            }
            else
            {
                var totalWidth = SpendingLimitBar.Width;
                var ratioSpendingLimitToSpending = Convert.ToDouble(currentSpending / SpendingLimit);
                CurrentSpendingBar.WidthRequest = totalWidth * ratioSpendingLimitToSpending;

                InvalidateLayout();
            }
        }

        /// <summary>
        ///     This method is necessary, so that the bar is loaded correctly for the items that are already visible in the list without scrolling.
        ///     Without those the first items you see would all have a empty progress bar until you scroll them out and in of the view again.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void UpdateCurrentSpendingBar(object sender, EventArgs eventArgs)
        {
            if (SpendingLimit == 0)
            {
                CurrentSpendingBar.WidthRequest = 0;
            }
            else
            {
                var totalWidth = SpendingLimitBar.Width;
                var ratioSpendingLimitToSpending = Convert.ToDouble(CurrentSpending / SpendingLimit);
                CurrentSpendingBar.WidthRequest = totalWidth * ratioSpendingLimitToSpending;

                InvalidateLayout();
            }
        }
    }

}
