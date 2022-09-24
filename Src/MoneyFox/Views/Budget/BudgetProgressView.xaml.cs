namespace MoneyFox.Views.Budget
{

    using System;

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
            get => (decimal)GetValue(CurrentSpendingProperty);
            set => SetValue(property: CurrentSpendingProperty, value: value);
        }

        public decimal SpendingLimit
        {
            get => (decimal)GetValue(SpendingLimitProperty);
            set => SetValue(property: SpendingLimitProperty, value: value);
        }

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
