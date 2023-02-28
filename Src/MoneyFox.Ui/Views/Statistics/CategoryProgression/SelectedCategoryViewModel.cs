namespace MoneyFox.Ui.Views.Statistics.CategoryProgression;

using CommunityToolkit.Mvvm.ComponentModel;

public class SelectedCategoryViewModel : ObservableObject
{
    private int id;
    private string name = "";

    public required int Id
    {
        get => id;
        set => SetProperty(field: ref id, newValue: value);
    }

    public required string Name
    {
        get => name;
        set => SetProperty(field: ref name, newValue: value);
    }
}
