namespace MoneyFox.Views.Accounts
{

    using Core.Resources;
    using ViewModels.Accounts;

    public partial class AddAccountPage
    {
        public AddAccountPage()
        {
            InitializeComponent();
            BindingContext = App.GetViewModel<AddAccountViewModel>();
            var cancelItem = new ToolbarItem
            {
                Command = new Command(async () => await Navigation.PopModalAsync()),
                Text = Strings.CancelLabel,
                Priority = -1,
                Order = ToolbarItemOrder.Primary
            };

            var saveItem = new ToolbarItem
            {
                Command = new Command(() => ViewModel.SaveCommand.Execute(null)),
                Text = Strings.SaveLabel,
                Priority = 1,
                Order = ToolbarItemOrder.Primary
            };

            ToolbarItems.Add(cancelItem);
            ToolbarItems.Add(saveItem);
        }

        private AddAccountViewModel ViewModel => (AddAccountViewModel)BindingContext;
    }

}
