namespace MoneyFox.Views.Statistics
{

    using ViewModels.Statistics;

    public partial class StatisticSelectorPage
    {
        public StatisticSelectorPage()
        {
            InitializeComponent();
            BindingContext = App.GetViewModel<StatisticSelectorViewModel>();
        }
    }

}
