namespace MoneyFox.Ui.Views;

using System.ComponentModel;
using System.Runtime.CompilerServices;

public class ObservableViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(sender: this, e: new(propertyName));
    }

    protected bool SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(x: property, y: value))
        {
            return false;
        }

        property = value;
        RaisePropertyChanged(propertyName);

        return true;
    }
}
