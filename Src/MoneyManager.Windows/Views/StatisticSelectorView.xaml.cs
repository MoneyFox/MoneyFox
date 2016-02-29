namespace MoneyManager.Windows.Views
{
    public sealed partial class StatisticSelectorView
    {
        public StatisticSelectorView()
        {
            InitializeComponent();

            DataContext = Mvx.Resolve<StatisticSelectorViewModel>();
        }
    }
}