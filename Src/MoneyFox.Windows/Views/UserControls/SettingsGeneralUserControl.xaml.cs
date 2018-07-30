using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace MoneyFox.Windows.Views.UserControls
{
    public sealed partial class SettingsGeneralUserControl
    {
        public SettingsGeneralUserControl()
        {
            InitializeComponent();
        }

        private void Sync_Click(object sender, RoutedEventArgs e)
        {
            Progress.Opacity = 1;
            Clipper.TranslateX = -30;
            Progress.Value = 0;
            FillProgressBar.Begin();
        }

        private void Progress_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (Progress.Value == 100)
            {
                ShowCheck.Begin();
                Progress.Opacity = 0;
            }
        }
    }
}