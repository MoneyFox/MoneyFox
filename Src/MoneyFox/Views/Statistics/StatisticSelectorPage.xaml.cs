namespace MoneyFox.Views.Statistic
{
    public partial class StatisticSelectorPage
    {
        public StatisticSelectorPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.StatisticSelectorViewModel;
        }
    }
}
