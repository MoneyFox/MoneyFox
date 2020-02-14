using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using MoneyFox.Domain.Entities;
using MoneyFox.Infrastructure.Tests.Collections;
using MoneyFox.Presentation.ViewModels;
using Should;
using Xunit;

namespace MoneyFox.Infrastructure.Tests.Mappings
{
    [ExcludeFromCodeCoverage]
    [Collection("AutoMapperCollection")]
    public class AccountMappingTests
    {
        private readonly IMapper mapper;

        public AccountMappingTests(MapperCollectionFixture fixture)
        {
            mapper = fixture.Mapper;
        }

        [Fact]
        public void MapToViewModel()
        {
            // Arrange
            var account = new Account("Testname", 40, "My Note", true);

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
