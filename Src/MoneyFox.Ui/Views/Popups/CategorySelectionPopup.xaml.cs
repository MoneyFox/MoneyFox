namespace MoneyFox.Ui.Views.Popups;

using ViewModels.Categories;

public partial class CategorySelectionPopup
{
    public CategorySelectionPopup()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<SelectCategoryViewModel>();
        Opened += async (sender, args) => await ViewModel.InitializeAsync();
    }

    private SelectCategoryViewModel ViewModel => (SelectCategoryViewModel)BindingContext;

    private void ClickClosePopup(object? sender, EventArgs e)
    {
        Close();
    }
}
