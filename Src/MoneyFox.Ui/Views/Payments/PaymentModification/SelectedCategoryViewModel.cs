namespace MoneyFox.Ui.Views.Payments.PaymentModification;

using CommunityToolkit.Mvvm.ComponentModel;

public class SelectedCategoryViewModel : ObservableObject
{
    private int id;
    private string name = "";
    private bool requireNote;

    public required int Id
    {
        get => id;
        set
        {
            if (id == value)
            {
                return;
            }

            id = value;
            OnPropertyChanged();
        }
    }

    public required string Name
    {
        get => name;
        set
        {
            if (name == value)
            {
                return;
            }

            name = value;
            OnPropertyChanged();
        }
    }

    public required bool RequireNote
    {
        get => requireNote;
        set
        {
            if (requireNote == value)
            {
                return;
            }

            requireNote = value;
            OnPropertyChanged();
        }
    }
}
