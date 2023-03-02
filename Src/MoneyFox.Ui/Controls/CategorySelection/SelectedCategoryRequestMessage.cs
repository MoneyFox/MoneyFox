namespace MoneyFox.Ui.Controls.CategorySelection;

using CommunityToolkit.Mvvm.Messaging.Messages;

internal class SelectedCategoryRequestMessage : RequestMessage<SelectedCategory?>
{
}

public record SelectedCategory(int Id, string Name);
