using Windows.UI.Xaml;

namespace MoneyManager.Windows.Controls
{
    public sealed partial class TabHeader
    {
        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register("Glyph", typeof (string),
            typeof (TabHeader), null);

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof (string),
            typeof (TabHeader), null);


        public TabHeader()
        {
            InitializeComponent();
            DataContext = this;
        }

        public string Glyph
        {
            get { return GetValue(GlyphProperty) as string; }
            set { SetValue(GlyphProperty, value); }
        }

        public string Label
        {
            get { return GetValue(LabelProperty) as string; }
            set { SetValue(LabelProperty, value); }
        }
    }
}