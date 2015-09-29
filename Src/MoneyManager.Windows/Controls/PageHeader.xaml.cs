using Windows.Foundation;
using Windows.UI.Xaml;

namespace MoneyManager.Windows.Controls
{
    public sealed partial class PageHeader
    {
        // Using a DependencyProperty as the backing store for HeaderContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderContentProperty =
            DependencyProperty.Register("HeaderContent", typeof (UIElement), typeof (PageHeader),
                new PropertyMetadata(DependencyProperty.UnsetValue));

        public PageHeader()
        {
            InitializeComponent();

            Loaded += (s, a) =>
            {
                AppShell.Current.TogglePaneButtonRectChanged += Current_TogglePaneButtonSizeChanged;
                TitleBar.Margin = new Thickness(AppShell.Current.TogglePaneButtonRect.Right, 0, 0, 0);
            };
        }

        public UIElement HeaderContent
        {
            get { return (UIElement) GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        private void Current_TogglePaneButtonSizeChanged(AppShell sender, Rect e)
        {
            TitleBar.Margin = new Thickness(e.Right, 0, 0, 0);
        }
    }
}