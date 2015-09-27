using System;
using Windows.UI.Xaml.Controls;

namespace MoneyManager.Windows
{
    /// <summary>
    ///     Data to represent an item in the nav menu.
    /// </summary>
    public class NavMenuItem
    {
        public string Label { get; set; }
        public Symbol Symbol { get; set; }

        public char SymbolAsChar => (char) Symbol;

        public Type DestPage { get; set; }
        public object Arguments { get; set; }
    }
}