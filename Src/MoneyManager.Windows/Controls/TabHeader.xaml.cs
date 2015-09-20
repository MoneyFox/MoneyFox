using Windows.UI.Xaml;

namespace MoneyManager.Windows.Controls
{
    public sealed partial class TabHeader
    {
        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register("Glyph", typeof(string), typeof(TabHeader), null);

        public string Glyph
        {
            get { return GetValue(GlyphProperty) as string; }
            set { SetValue(GlyphProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(TabHeader), null);

        public string Label
        {
            get { return GetValue(LabelProperty) as string; }
            set { SetValue(LabelProperty, value); }
        }


        public TabHeader()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
