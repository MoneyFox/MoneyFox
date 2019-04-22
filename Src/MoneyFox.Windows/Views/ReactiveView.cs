using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ReactiveUI;

namespace MoneyFox.Windows.Views
{
    public abstract class ReactiveView<TViewModel> : Page, IViewFor<TViewModel>
        where TViewModel : class
    {
        /// <summary>
        /// The view model dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(TViewModel),
                typeof(ReactiveUserControl<TViewModel>),
                new PropertyMetadata(null));

        /// <inheritdoc/>
        public TViewModel ViewModel
        {
            get => (TViewModel) GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        /// <inheritdoc/>
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (TViewModel) value;
        }
    }
}