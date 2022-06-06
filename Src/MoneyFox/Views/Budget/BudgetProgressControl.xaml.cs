namespace MoneyFox.Views.Budget
{

    using Xamarin.Forms;

    public partial class BudgetProgressControl
    {
        public static readonly BindableProperty CurrentSpendingProperty = BindableProperty.Create(
            propertyName: "CurrentSpending",
            returnType: typeof(decimal),
            declaringType: typeof(BudgetProgressControl));

        public static readonly BindableProperty SpendingLimitProperty = BindableProperty.Create(
            propertyName: "SpendingLimit",
            returnType: typeof(decimal),
            declaringType: typeof(BudgetProgressControl));

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
    }

}
