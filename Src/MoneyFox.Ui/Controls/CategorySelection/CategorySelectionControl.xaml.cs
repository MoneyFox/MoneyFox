namespace MoneyFox.Ui.Controls.CategorySelection;

public partial class CategorySelectionControl : ContentView
{
	public CategorySelectionControl()
	{
		InitializeComponent();
        BindingContext = App.GetViewModel<CategorySelectionViewModel>();
	}
}
