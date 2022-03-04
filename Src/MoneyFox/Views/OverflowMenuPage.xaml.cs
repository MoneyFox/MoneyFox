namespace MoneyFox.Views
{
    using Xamarin.Forms.Xaml;

    public partial class OverflowMenuPage
    {
        public OverflowMenuPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.OverflowMenuViewModel;
        }
    }
}