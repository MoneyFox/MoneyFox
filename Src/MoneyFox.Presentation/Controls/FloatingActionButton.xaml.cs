using Xamarin.Forms;

namespace MoneyFox.Presentation.Controls
{
    public partial class FloatingActionButton : Button
    {
        public static BindableProperty ButtonColorProperty = BindableProperty.Create(nameof(ButtonColor), typeof(Color),
            typeof(FloatingActionButton), Color.Accent);

        public FloatingActionButton()
        {
            InitializeComponent();
        }

        public Color ButtonColor
        {
            get => (Color) GetValue(ButtonColorProperty);
            set => SetValue(ButtonColorProperty, value);
        }
    }
}