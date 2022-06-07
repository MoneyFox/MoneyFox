namespace MoneyFox.Views.Budget
{

    using System;
    using Xamarin.Forms;

    public partial class BudgetProgressControl
    {
        public static readonly BindableProperty CurrentSpendingProperty = BindableProperty.Create(
            propertyName: "CurrentSpending",
            returnType: typeof(decimal),
            declaringType: typeof(BudgetProgressControl),
            defaultValue: 0m);

        public static readonly BindableProperty SpendingLimitProperty = BindableProperty.Create(
            propertyName: "SpendingLimit",
            returnType: typeof(decimal),
            declaringType: typeof(BudgetProgressControl),
            defaultValue: 0m);

        public BudgetProgressControl()
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

        private void UpdateCurrentSpendingBar(object sender, EventArgs e)
        {
            var totalWidth = SpendingLimitBar.Width;
            var ratioSpendingLimitToSpending = Convert.ToDouble(CurrentSpending / SpendingLimit);
            CurrentSpendingBar.WidthRequest = totalWidth * ratioSpendingLimitToSpending;
            InvalidateLayout();
        }
    }

}
