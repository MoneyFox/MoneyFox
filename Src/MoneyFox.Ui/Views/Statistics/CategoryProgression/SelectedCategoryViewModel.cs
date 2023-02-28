namespace MoneyFox.Ui.Views.Statistics.CategoryProgression;

using CommunityToolkit.Mvvm.ComponentModel;

public class SelectedCategoryViewModel : ObservableObject
{
    private int id;
    private string name = "";

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
}
