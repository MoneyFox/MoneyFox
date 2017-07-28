using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using FontAwesome.UWP;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Windows
{
    public class ShellNavigationItem : INotifyPropertyChanged
    {
        private bool isSelected;

        private Visibility _selectedVis = Visibility.Collapsed;
        public Visibility SelectedVis
        {
            get => _selectedVis;
            set
            {
                if (_selectedVis == value) return;

                _selectedVis = value;
                RaisePropertyChanged();
            }
        }

        public string Label { get; set; }
        public FontAwesomeIcon Symbol { get; set; }
        public Type ViewModel { get; set; }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected == value) return;

                isSelected = value;
                RaisePropertyChanged();

                SelectedVis = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private SolidColorBrush GetStandardTextColorBrush()
        {
            var brush = Application.Current.Resources["SystemControlForegroundBaseHighBrush"] as SolidColorBrush;
            return brush;
        }


        private ShellNavigationItem(string name, FontAwesomeIcon symbol, Type pageType)
        {
            Label = name;
            Symbol = symbol;
            ViewModel = pageType;
        }

        public static ShellNavigationItem FromType<T>(string name, FontAwesomeIcon symbol) where T : IMvxViewModel
        {
            return new ShellNavigationItem(name, symbol, typeof(T));
        }

        /// <summary>
        ///     Invoked when the value of a property changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Update observer after a property changed
        /// </summary>
        /// <param name="propertyName">Name of the cahnged property.</param>
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
