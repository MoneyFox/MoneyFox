
using MoneyFox.Foundation.DataModels;
using Ploeh.AutoFixture;
using Xunit;
using XunitShouldExtension;

namespace MoneyFox.Foundation.Tests.DataModels
{
    public class PaymentViewModelTests
    {
        [Fact]
        public void Category_SetId()
        {
            var paymentVm = new PaymentViewModel();
            paymentVm.Category.ShouldBeNull();
            paymentVm.CategoryId.ShouldBeNull();

            var category = new Fixture().Create<CategoryViewModel>();
            paymentVm.Category = category;

            paymentVm.Category.ShouldBe(category);
            paymentVm.Category.Id.ShouldBe(category.Id);
            paymentVm.CategoryId.ShouldBe(category.Id);
        }
    }
}
