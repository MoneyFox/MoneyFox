namespace MoneyFox.Ui.Views.Categories.ModifyCategory;

using Common.Navigation;

[QueryProperty(name: "CategoryId", queryId: "categoryId")]
public partial class EditCategoryPage : IBindablePage
{
    public EditCategoryPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<EditCategoryViewModel>();
    }

    private EditCategoryViewModel ViewModel => (EditCategoryViewModel)BindingContext;

    protected override void OnAppearing()
    {
        ViewModel.IsActive = true;
        ViewModel.InitializeAsync(categoryId).GetAwaiter().GetResult();
    }

    protected override void OnDisappearing()
    {
        ViewModel.IsActive = false;
    }

#pragma warning disable S2376 // Write-only properties should not be used
    private int categoryId;
    public string CategoryId
    {
        set => categoryId = Convert.ToInt32(Uri.UnescapeDataString(value));
    }
#pragma warning restore S2376 // Write-only properties should not be used
}
