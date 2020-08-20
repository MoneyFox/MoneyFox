namespace MoneyFox.Views.Statistics
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
