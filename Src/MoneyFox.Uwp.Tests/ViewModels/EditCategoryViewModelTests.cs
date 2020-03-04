using AutoMapper;
using MediatR;
using MoneyFox.Application.Categories.Queries.GetCategoryById;
using MoneyFox.Application.Resources;
using MoneyFox.Domain.Entities;
using MoneyFox.Uwp.ViewModels;
using Moq;
using Should;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Presentation.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class EditCategoryViewModelTests
    {
        [Fact]
        public async Task Initialize_MapperCalled()
        {
            // Arrange
            const int categoryId = 99;
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetCategoryByIdQuery>(), default)).ReturnsAsync(new Category("asd"));

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<CategoryViewModel>(It.IsAny<Category>())).Returns(new CategoryViewModel());

            var editAccountVm = new EditCategoryViewModel(mediatorMock.Object, null, null, mapperMock.Object);

            // Act
            editAccountVm.CategoryId = categoryId;
            await editAccountVm.InitializeCommand.ExecuteAsync();

            // Assert
            mapperMock.Verify(x => x.Map<CategoryViewModel>(It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public async Task Initialize_CategoryLoaded()
        {
            // Arrange
            const int categoryId = 99;
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetCategoryByIdQuery>(), default)).ReturnsAsync(new Category("asd"));

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<CategoryViewModel>(It.IsAny<Category>())).Returns(new CategoryViewModel());

            var editAccountVm = new EditCategoryViewModel(mediatorMock.Object, null, null, mapperMock.Object);

            // Act
            editAccountVm.CategoryId = categoryId;
            await editAccountVm.InitializeCommand.ExecuteAsync();

            // Assert
            mediatorMock.Verify(x => x.Send(It.IsAny<GetCategoryByIdQuery>(), default), Times.Once);
        }

        [Fact]
        public async Task Initialize_Title_Set()
        {
            // Arrange
            const int categoryId = 99;
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetCategoryByIdQuery>(), default)).ReturnsAsync(new Category("asd"));

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<CategoryViewModel>(It.IsAny<Category>())).Returns(new CategoryViewModel());

            var editAccountVm = new EditCategoryViewModel(mediatorMock.Object, null, null, mapperMock.Object);

            // Act
            editAccountVm.CategoryId = categoryId;
            await editAccountVm.InitializeCommand.ExecuteAsync();

            // Assert
            editAccountVm.Title.ShouldContain(Strings.EditCategoryTitle);
        }
    }
}
