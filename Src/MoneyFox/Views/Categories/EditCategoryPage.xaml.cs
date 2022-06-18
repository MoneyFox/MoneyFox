namespace MoneyFox.Views.Categories
{

    using CommonServiceLocator;
    using Core.Resources;
    using ViewModels.Categories;
    using Xamarin.Forms;

    public partial class EditCategoryPage
    {
        private readonly int categoryId;

        public EditCategoryPage(int categoryId)
        {
            InitializeComponent();
            BindingContext = App.GetViewModel<EditCategoryViewModel>();
            this.categoryId = categoryId;
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

        private EditCategoryViewModel ViewModel => (EditCategoryViewModel)BindingContext;

        protected override async void OnAppearing()
        {
            await ViewModel.InitializeAsync(categoryId);
        }
    }

}
