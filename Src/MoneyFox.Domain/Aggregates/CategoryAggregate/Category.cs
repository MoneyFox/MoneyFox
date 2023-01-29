namespace MoneyFox.Domain.Aggregates.CategoryAggregate;

using AccountAggregate;
using JetBrains.Annotations;

public class Category : EntityBase
{
    [UsedImplicitly]
    private Category() { }

    public Category(string name, string? note = null, bool requireNote = false)
    {
        UpdateData(name: name, note: note, requireNote: requireNote);
    }

    public int Id
    {
        get;

        [UsedImplicitly]
        private set;
    }

    public string Name { get; private set; } = "";

    public string? Note { get; private set; }

    public bool RequireNote { get; private set; }

    public List<Payment> Payments
    {
        get;

        [UsedImplicitly]
        private set;
    } = new();

    public void UpdateData(string name, string? note = "", bool requireNote = false)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        Name = name;
        Note = note;
        RequireNote = requireNote;
    }
}
