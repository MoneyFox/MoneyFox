namespace MoneyFox.Ui.Views.Payments.PaymentModification;

public class SelectedCategoryViewModel : ObservableViewModelBase
{
    private int id;
    private string name = "";
    private bool requireNote;

    public required int Id
    {
        get => id;
        set => SetProperty(property: ref id, value: value);
    }

    public required string Name
    {
        get => name;
        set => SetProperty(property: ref name, value: value);
    }

    public required bool RequireNote
    {
        get => requireNote;
        set => SetProperty(property: ref requireNote, value: value);
    }
}
