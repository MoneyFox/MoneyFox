using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Tests;
using Ploeh.AutoFixture;
using Xunit;

namespace MoneyFox.Business.Tests.DataModels
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

        [Fact]
        public void ChargedAccount_SetId()
        {
            var paymentVm = new PaymentViewModel();
            paymentVm.ChargedAccount.ShouldBeNull();
            paymentVm.ChargedAccountId.ShouldBe(0);

            var fixture = new Fixture();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var accountViewModel = fixture.Create<AccountViewModel>();
            paymentVm.ChargedAccount = accountViewModel;

            paymentVm.ChargedAccount.ShouldBe(accountViewModel);
            paymentVm.ChargedAccount.Id.ShouldBe(accountViewModel.Id);
            paymentVm.ChargedAccountId.ShouldBe(accountViewModel.Id);
        }

        [Fact]
        public void TargetAccount_SetId()
        {
            var paymentVm = new PaymentViewModel();
            paymentVm.TargetAccount.ShouldBeNull();
            paymentVm.TargetAccountId.ShouldBe(0);

            var fixture = new Fixture();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var accountViewModel = fixture.Create<AccountViewModel>();
            paymentVm.TargetAccount = accountViewModel;

            paymentVm.TargetAccount.ShouldBe(accountViewModel);
            paymentVm.TargetAccount.Id.ShouldBe(accountViewModel.Id);
            paymentVm.TargetAccountId.ShouldBe(accountViewModel.Id);
        }
    }
}
