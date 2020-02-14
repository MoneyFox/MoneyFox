using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using MoneyFox.Domain.Entities;
using MoneyFox.Infrastructure.Tests.Collections;
using MoneyFox.Presentation.ViewModels;
using Should;
using Xunit;

namespace MoneyFox.Presentation.Tests.Mapper
{
    [ExcludeFromCodeCoverage]
    [Collection("AutoMapperCollection")]
    public class AccountViewModelMapperTests
    {
        private readonly IMapper mapper;

        public AccountViewModelMapperTests(MapperCollectionFixture fixture)
        {
            mapper = fixture.Mapper;
        }

        [Fact]
        public void AccountMappedToCorrectType()
        {
            // Arrange
            var account = new Account("test");

            // Act
            var result = mapper.Map<AccountViewModel>(account);

            // Assert
            result.ShouldBeType<AccountViewModel>();
        }

        [Fact]
        public void AccountMappedToCorrectFields()
        {
            // Arrange
            var account = new Account("test", 123, "TestNote", true);

            // Act
            var result = mapper.Map<AccountViewModel>(account);

            // Assert
            result.Name.ShouldEqual(account.Name);
            result.CurrentBalance.ShouldEqual(account.CurrentBalance);
            result.Note.ShouldEqual(account.Note);
            result.IsExcluded.ShouldEqual(account.IsExcluded);
        }
    }
}
