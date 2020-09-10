using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MoneyFox.Ui.Shared
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public bool IsEmpty { get; set; }

        virtual public void Merge(ObservableObject source) { }

        protected bool Set<T>(ref T field, T newValue = default(T), [CallerMemberName] string propertyName = null)
        {
            if(!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                NotifyPropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NotifyChanges()
        {
            // Notify all properties
            NotifyPropertyChanged("");
        }
    }
}
