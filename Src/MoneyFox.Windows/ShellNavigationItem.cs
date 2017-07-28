using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Windows
{
    public class ShellNavigationItem : INotifyPropertyChanged
    {
        private bool _isSelected;

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

        private SolidColorBrush _selectedForeground = null;
        public SolidColorBrush SelectedForeground
        {
            get => _selectedForeground ?? (_selectedForeground = GetStandardTextColorBrush());
            set
            {
                if (_selectedForeground == value) return;

                _selectedForeground = value;
                RaisePropertyChanged();
            }
        }

        public string Label { get; set; }
        public Symbol Symbol { get; set; }
        public char SymbolAsChar { get { return (char)Symbol; } }
        public Type ViewModel { get; set; }

        private readonly IconElement iconElement = null;
        public IconElement Icon
        {
            get
            {
                var foregroundBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath("SelectedForeground"),
                    Mode = BindingMode.OneWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };

                if (iconElement != null)
                {
                    BindingOperations.SetBinding(iconElement, IconElement.ForegroundProperty, foregroundBinding);

                    return iconElement;
                }

                var fontIcon = new FontIcon { FontSize = 16, Glyph = SymbolAsChar.ToString() };

                BindingOperations.SetBinding(fontIcon, FontIcon.ForegroundProperty, foregroundBinding);

                return fontIcon;
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;

                _isSelected = value;
                RaisePropertyChanged();

                SelectedVis = value ? Visibility.Visible : Visibility.Collapsed;
                SelectedForeground = value
                    ? Application.Current.Resources["SystemControlForegroundAccentBrush"] as SolidColorBrush
                    : GetStandardTextColorBrush();
            }
        }

        private SolidColorBrush GetStandardTextColorBrush()
        {
            var brush = Application.Current.Resources["SystemControlForegroundBaseHighBrush"] as SolidColorBrush;

            return brush;
        }

        private ShellNavigationItem(string name, Symbol symbol, Type pageType)
        {
            Label = name;
            Symbol = symbol;
            ViewModel = pageType;
        }

        private ShellNavigationItem(string name, IconElement icon, Type pageType)
        {
            Label = name;
            iconElement = icon;
            ViewModel = pageType;
        }

        public static ShellNavigationItem FromType<T>(string name, Symbol symbol) where T : IMvxViewModel
        {
            return new ShellNavigationItem(name, symbol, typeof(T));
        }

        public static ShellNavigationItem FromType<T>(string name, IconElement icon) where T : IMvxViewModel
        {
            return new ShellNavigationItem(name, icon, typeof(T));
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
