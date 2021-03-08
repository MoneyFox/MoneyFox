using MoneyFox.Uwp.Src;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Uwp.Tests
{
    [ExcludeFromCodeCoverage]
    public class DialogServiceTests
    {
        [Fact]
        public async Task OpenAndHideLoadingDialog()
        {
            // Arrange
            var dialogService = new DialogService();

            // Act
            await dialogService.ShowLoadingDialogAsync();
            await dialogService.HideLoadingDialogAsync();
        }
    }
}
