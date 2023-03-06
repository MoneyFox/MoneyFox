namespace MoneyFox.Ui.Controls.CategorySelection;

using CommunityToolkit.Mvvm.ComponentModel;
using Views;

public class SelectedCategoryViewModel : ObservableObject
{
    private int id;
    private string name = "";
    private bool requireNote;

    public required int Id
    {
        get => id;
        set => SetProperty(ref id, value);
    }

    public required string Name
    {
        get => name;
        set => SetProperty(ref name, value);
    }

    public required bool RequireNote
    {
        get => requireNote;
        set => SetProperty(ref requireNote, value);
    }
}
