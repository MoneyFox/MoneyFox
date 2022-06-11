namespace MoneyFox.Views.Budget
{

    using System;
    using Core.Resources;
    using Extensions;
    using ViewModels.Budget;
    using Xamarin.Forms;

    public partial class AddBudgetPage : ContentPage
    {
        public AddBudgetPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.AddBudgetViewModel;
            var cancelItem = new ToolbarItem
            {
                Command = new Command(async () => await Navigation.PopModalAsync()),
                Text = Strings.CancelLabel,
                Priority = -1,
                Order = ToolbarItemOrder.Primary
            };

            var saveItem = new ToolbarItem
            {
                Command = new Command(() => ViewModel.SaveBudgetCommand.Execute(null)),
                Text = Strings.SaveLabel,
                Priority = 1,
                Order = ToolbarItemOrder.Primary
            };

            ToolbarItems.Add(cancelItem);
            ToolbarItems.Add(saveItem);
        }

        private AddBudgetViewModel ViewModel => (AddBudgetViewModel)BindingContext;
    }

}
