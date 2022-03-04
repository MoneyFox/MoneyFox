namespace MoneyFox.Views.OverflowMenu
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