using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Media;

namespace MoneyFox.Windows.Views
{

    public class PropertyMonitor : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        // Monitor the window size to create a dynamic spacer
        private int _spacer;

        public int Spacer
        {
            get { return _spacer; }
            set
            {
                _spacer = value;
                NotifyPropertyChanged();
            }
        }
    }
}
