using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MoneyFox.Foundation.Models
{
    public class StatisticItem : INotifyPropertyChanged
    {
        private string label;
        private double value;
        private double percentage;

        /// <summary>
        ///     Value of this item
        /// </summary>
        public double Value
        {
            get => value;
            set
            {
                if (Math.Abs(this.value - value) < 0.01) return;
                this.value = Math.Round(value, 2); ;
                RaisePropertyChanged();
            }

        }
        /// <summary>
        ///     Value of this item
        /// </summary>
        public double Percentage
        {
            get => percentage;
            set
            {
                if (Math.Abs(this.value - value) < 0.01) return;
                percentage = Math.Round(value, 2);
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Label to show in the chart
        /// </summary>
        public string Label
        {
            get => label;
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