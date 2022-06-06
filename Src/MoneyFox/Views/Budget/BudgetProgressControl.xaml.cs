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
            defaultValue: 0m,
            propertyChanged: CurrentSpendingChanged);

        public static readonly BindableProperty SpendingLimitProperty = BindableProperty.Create(
            propertyName: "SpendingLimit",
            returnType: typeof(decimal),
            declaringType: typeof(BudgetProgressControl),
            defaultValue: 0m,
            propertyChanged: CurrentSpendingChanged);

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

        private static void CurrentSpendingChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var control = bindable as BudgetProgressControl;
            control.UpdateProgressBar();
        }

        private void UpdateProgressBar()
        {
            if (SpendingLimit == 0 || SpendingLimitBar == null)
            {
                return;
            }

            var totalWidth = SpendingLimitBar.Width;
            var ratioSpendingLimitToSpending = Convert.ToDouble(CurrentSpending / SpendingLimit);
            CurrentSpendingBar.WidthRequest = totalWidth * ratioSpendingLimitToSpending;
        }
    }

}
