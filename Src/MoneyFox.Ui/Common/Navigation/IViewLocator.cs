namespace MoneyFox.Ui.Common.Navigation;

public interface IViewLocator
{
    IBindablePage GetViewFor<TViewModel>() where TViewModel : NavigableViewModel;

    IBindablePage GetView<TView>() where TView : class, IBindablePage;

    Type GetViewTypeFor<TViewModel>() where TViewModel : NavigableViewModel;
}
