namespace MoneyFox.Ui.Views.Setup;

using Common.Navigation;

public partial class SetupCategoryPage: IBindablePage
{
    public SetupCategoryPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<SetupCategoryViewModel>();
    }
}
