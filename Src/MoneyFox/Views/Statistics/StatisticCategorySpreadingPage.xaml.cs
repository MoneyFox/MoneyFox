namespace MoneyFox.Views.Statistics
{
    using Dialogs;
    using System;
    using ViewModels.Statistics;

    public partial class StatisticCategorySpreadingPage
    {
        public StatisticCategorySpreadingPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.StatisticCategorySpreadingViewModel;

            ViewModel.LoadedCommand.Execute(null);
        }

        private StatisticCategorySpreadingViewModel ViewModel => (StatisticCategorySpreadingViewModel)BindingContext;

        private async void OpenFilterDialog(object sender, EventArgs e)
            => await new DateSelectionPopup(ViewModel.StartDate, ViewModel.EndDate).ShowAsync();
    }
}