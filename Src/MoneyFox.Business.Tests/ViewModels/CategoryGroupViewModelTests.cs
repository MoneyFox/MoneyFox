using AutoFixture;
using MoneyFox.Business.ViewModels;
using MoneyFox.DataAccess.Pocos;
using Should;
using Xunit;

namespace MoneyFox.Business.Tests.ViewModels
{
    public class CategoryGroupViewModelTests
    {
        [Fact]
        public void Name_CorrectNameReturnedAndSet()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var group = fixture.Create<CategoryGroup>();
            var vm = new CategoryGroupViewModel(group);

            // Act / Assert
            vm.Name.ShouldEqual(group.Data.Name);
        }

        [Fact]
        public void Note_CorrectNameReturnedAndSet()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var group = fixture.Create<CategoryGroup>();
            var vm = new CategoryGroupViewModel(group);

            // Act / Assert
            vm.Note.ShouldEqual(group.Data.Note);
        }
    }
}
