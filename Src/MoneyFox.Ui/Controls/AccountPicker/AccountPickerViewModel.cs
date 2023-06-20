namespace MoneyFox.Ui.Controls.AccountPicker;

using Domain;

public record AccountPickerViewModel(int Id, string Name, Money CurrentBalance);
