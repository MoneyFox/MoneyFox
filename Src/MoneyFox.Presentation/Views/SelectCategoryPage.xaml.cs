using MoneyFox.Application.Resources;
using MoneyFox.Presentation.Utilities;
using MoneyFox.Presentation.ViewModels;
using System;
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

            if (Device.RuntimePlatform == Device.iOS)
            {
                var addItem = new ToolbarItem
                {
                    Text = Strings.AddTitle,
                    Priority = -1,
                    Order = ToolbarItemOrder.Primary
                };
                addItem.Clicked += AddCategoryClick;
                ToolbarItems.Add(addItem);
            }

            var cancelItem = new ToolbarItem
            {
                Command = new Command(async () => await Close()),
                Text = Strings.CancelLabel,
                Priority = 0,
                Order = ToolbarItemOrder.Primary
            };

            ToolbarItems.Add(cancelItem);
        }

        protected override void OnAppearing()
        {
            ViewModel.AppearingCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }

        private void AddCategoryClick(object sender, EventArgs e)
        {
            ViewModel.CreateNewCategoryCommand.Execute(null);
        }

        private async Task Close()
        {
            await Navigation.PopModalAsync();
        }
    }
}
