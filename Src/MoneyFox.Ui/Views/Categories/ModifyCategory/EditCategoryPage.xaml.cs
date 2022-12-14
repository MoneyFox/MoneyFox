namespace MoneyFox.Ui.Views.Categories.ModifyCategory;

[QueryProperty(name: "CategoryId", queryId: "categoryId")]
public partial class EditCategoryPage
{
    public EditCategoryPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<EditCategoryViewModel>();
    }

    private EditCategoryViewModel ViewModel => (EditCategoryViewModel)BindingContext;

    protected override async void OnAppearing()
    {
        await ViewModel.InitializeAsync(categoryId);
    }

#pragma warning disable S2376 // Write-only properties should not be used
    private int categoryId;
    public string CategoryId
    {
        set => categoryId = Convert.ToInt32(Uri.UnescapeDataString(value));
    }
#pragma warning restore S2376 // Write-only properties should not be used
}



