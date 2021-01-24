using MoneyFox.Presentation.Dialogs;
using MoneyFox.ViewModels.Statistics;
using System;

namespace MoneyFox.Views.Statistics
{
    public partial class StatisticCategorySpreadingPage
    {
        private StatisticCategorySpreadingViewModel ViewModel => (StatisticCategorySpreadingViewModel)BindingContext;

        public StatisticCategorySpreadingPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.StatisticCategorySpreadingViewModel;

            ViewModel.LoadedCommand.Execute(null);
        }

        private static async void OpenFilterDialog(object sender, EventArgs e)
            => await new DateSelectionPopup().ShowAsync();
    }
}
