using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MoneyFox.Shared.Model
{
    public class StatisticItem : INotifyPropertyChanged
    {
        private string category;
        private string label;
        private double value;

        /// <summary>
        ///     Value used to group the items
        /// </summary>
        public string Category
        {
            get { return category; }
            set
            {
                if (category == value) return;
                category = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Value of this item
        /// </summary>
        public double Value
        {
            get { return value; }
            set
            {
                if (value == value) return;
                value = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Label to show in the chart
        /// </summary>
        public string Label
        {
            get { return label; }
            set
            {
                if (label == value) return;
                label = value;
                RaisePropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}