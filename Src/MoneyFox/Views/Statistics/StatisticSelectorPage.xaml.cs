namespace MoneyFox.Views
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
