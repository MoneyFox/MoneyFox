using System.Threading.Tasks;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Dialogs;
using MoneyFox.Presentation.Utilities;
using MoneyFox.Presentation.ViewModels.Statistic;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace MoneyFox.Presentation.Views
{
	public partial class StatisticCategorySummaryPage
    {
        private StatisticCategorySummaryViewModel ViewModel => BindingContext as StatisticCategorySummaryViewModel;

        public StatisticCategorySummaryPage ()
		{
            InitializeComponent();
            BindingContext = ViewModelLocator.StatisticCategorySummaryVm;

            Title = Strings.CategorySummaryTitle;

		    var filterItem = new ToolbarItem
		    {
		        Command = new Command(async () => await OpenDialog()),
		        Text = Strings.SelectDateLabel,
		        Priority = 0,
		        Order = ToolbarItemOrder.Primary
		    };

		    ToolbarItems.Add(filterItem);

            ViewModel.LoadedCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }

        private async Task OpenDialog()
        {
            await Navigation.PushPopupAsync(new DateSelectionPopup
            {
                BindingContext = ViewModelLocator.SelectDateRangeDialogVm
            });
        }
    }
}
