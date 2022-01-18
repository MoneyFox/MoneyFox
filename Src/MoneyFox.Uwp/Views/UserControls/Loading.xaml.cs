using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MoneyFox.Uwp.Views.UserControls
{
    public sealed partial class Loading : UserControl
    {
        public Loading()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register(
          "IsLoading",
          typeof(bool),
          typeof(Loading),
          new PropertyMetadata(false)
        );

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }
    }
}
