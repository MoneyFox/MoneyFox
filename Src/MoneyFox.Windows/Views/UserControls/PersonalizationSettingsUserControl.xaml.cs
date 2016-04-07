using MoneyFox.Shared.ViewModels;
using MvvmCross.Platform;

namespace MoneyFox.Windows.Views.UserControls
{
    public sealed partial class PersonalizationSettingsUserControl 
    {
        public PersonalizationSettingsUserControl()
        {
            InitializeComponent();

            DataContext = Mvx.Resolve<PersonalizationUserControlViewModel>();
        }
    }
}
