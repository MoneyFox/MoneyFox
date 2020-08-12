using MoneyFox.Application.Common;
using MoneyFox.Application.Resources;
using MoneyFox.Presentation.Dialogs;
using MoneyFox.Presentation.ViewModels.Statistic;
using Rg.Plugins.Popup.Extensions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.Views
{
    public partial class StatisticCategorySummaryPage
    {
        private StatisticCategorySummaryViewModel ViewModel => BindingContext as StatisticCategorySummaryViewModel;

        public StatisticCategorySummaryPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.StatisticCategorySummaryViewModel;

            var filterItem = new ToolbarItem
                             {
                                 Command = new Command(async() => await OpenDialog()),
                                 Text = Strings.SelectDateLabel,
                                 Priority = 0,
                                 Order = ToolbarItemOrder.Primary
                             };

            ToolbarItems.Add(filterItem);

            ViewModel.LoadedCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }

        private async Task OpenDialog()
        {
            await Navigation.PushPopupAsync(new DateSelectionPopup());
        }
    }
}
