using System.Diagnostics.CodeAnalysis;
using GenericServices;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Parameters;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.ServiceLayer.ViewModels;
using Moq;
using Should;
using Xunit;

namespace MoneyFox.Presentation.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class AddAccountViewModelTests
    {
        [Fact]
        public void Prepare_CategoryCreated()
        {
            // Arrange
            var crudServiceMock = new Mock<ICrudServicesAsync>();

            var addAccountVm = new AddAccountViewModel(crudServiceMock.Object, null, null, null, null, null);

            // Act
            addAccountVm.Prepare(new ModifyAccountParameter());

            // Assert
            addAccountVm.SelectedAccount.ShouldNotBeNull();
        }

        [Fact]
        public void Prepare_Title_Set()
        {
            // Arrange
            var crudServiceMock = new Mock<ICrudServicesAsync>();

            var addAccountVm = new AddAccountViewModel(crudServiceMock.Object, null, null, null, null, null);

            // Act
            addAccountVm.Prepare(new ModifyAccountParameter());

            // Assert
            addAccountVm.Title.ShouldEqual(Strings.AddAccountTitle);
        }
    }
}
