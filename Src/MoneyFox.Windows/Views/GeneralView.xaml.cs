using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Xamarin.Forms.Platform.UWP;
using MoneyFox.Views;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace MoneyFox.Windows.Views
{
    public sealed partial class GeneralView
    {
        public GeneralView()
        {
           InitializeComponent();
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < GeneralViewPivot.Items.Count; i++)
            {
                if (i == GeneralViewPivot.SelectedIndex)
                {
                    PivotItem selectedPivotItem = GeneralViewPivot.SelectedItem as PivotItem;
                    (selectedPivotItem.Header as TextBlock).Foreground = new SolidColorBrush(Color.FromArgb(255, 240, 146, 36));

                }
                else
                {
                    PivotItem unselectedPivotItem = GeneralViewPivot.Items[i] as PivotItem;
                    (unselectedPivotItem.Header as TextBlock).Foreground = new SolidColorBrush(Colors.DarkGray);
                }
            }
        }
    }
}
