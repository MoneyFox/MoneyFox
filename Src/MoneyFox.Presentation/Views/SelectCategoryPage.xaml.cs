using MoneyFox.Application.Resources;
using MoneyFox.Presentation.Utilities;
using MoneyFox.Presentation.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.Presentation.Views
{
    public partial class SelectCategoryPage
    {
        private SelectCategoryListViewModel ViewModel => BindingContext as SelectCategoryListViewModel;

        public SelectCategoryPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.SelectCategoryListVm;

            Title = Strings.SelectCategoryTitle;

            var filterItem = new ToolbarItem
            {
                Command = new Command(async () => await Close()),
                Text = Strings.CancelLabel,
                Priority = 0,
                Order = ToolbarItemOrder.Primary
            };

            ToolbarItems.Add(filterItem);
        }

        protected override void OnAppearing()
        {
            ViewModel.AppearingCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }

        private async Task Close()
        {
            await Navigation.PopModalAsync();
        }
    }
}
