namespace MoneyFox.Ui.Views.Payments;

using CommunityToolkit.Mvvm.ComponentModel;

public class SelectedCategoryViewModel : ObservableObject
{
    private int id;
    private string name = "";
    private bool requireNote;

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

    public required bool RequireNote
    {
        get => requireNote;
        set => SetProperty(field: ref requireNote, newValue: value);
    }
}
