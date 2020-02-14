using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using MoneyFox.Domain.Entities;
using MoneyFox.Presentation.Tests.Collections;
using MoneyFox.Presentation.ViewModels;
using Should;
using Xunit;

namespace MoneyFox.Presentation.Tests.Mappings
{
    [ExcludeFromCodeCoverage]
    [Collection("AutoMapperCollection")]
    public class CategoryMappingTests
    {
        private readonly IMapper mapper;

        public CategoryMappingTests(MapperCollectionFixture fixture)
        {
            mapper = fixture.Mapper;
        }

        [Fact]
        public void MapToViewModel()
        {
            // Arrange
            var category = new Category("Testname", "My Note");

            // Act
            var result = mapper.Map<CategoryViewModel>(category);

            // Assert
            result.Name.ShouldEqual(category.Name);
            result.Note.ShouldEqual(category.Note);
        }
    }
}
