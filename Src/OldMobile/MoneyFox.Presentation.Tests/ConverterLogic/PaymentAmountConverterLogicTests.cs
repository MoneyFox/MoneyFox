using MediatR;
using MoneyFox.Application;
using MoneyFox.Domain;
using MoneyFox.Presentation.ConverterLogic;
using MoneyFox.Presentation.Services;
using MoneyFox.Presentation.ViewModels;
using Moq;
using Should;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Xunit;

namespace MoneyFox.Presentation.Tests.ConverterLogic
{
    [ExcludeFromCodeCoverage]
    [Collection("MvxIocCollection")]
    public class PaymentAmountConverterLogicTests
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<INavigationService> navigationService;

        public PaymentAmountConverterLogicTests()
        {
            mediatorMock = new Mock<IMediator>();
            navigationService = new Mock<INavigationService>();
        }

        [Fact]
        public void Converter_Payment_NegativeAmountSign()
        {
            CultureHelper.CurrentCulture = new CultureInfo("en-US");
            PaymentAmountConverterLogic
               .GetFormattedAmountString(new PaymentViewModel(mediatorMock.Object, navigationService.Object) { Amount = 80, Type = PaymentType.Expense },
                                         string.Empty)
               .ShouldEqual($"- $80.00");
            CultureHelper.CurrentCulture = CultureInfo.CurrentCulture;
        }

        [Fact]
        public void Converter_Payment_PositiveAmountSign()
        {
            CultureHelper.CurrentCulture = new CultureInfo("en-US");
            PaymentAmountConverterLogic
               .GetFormattedAmountString(new PaymentViewModel(mediatorMock.Object, navigationService.Object) { Amount = 80, Type = PaymentType.Income },
                                         string.Empty)
               .ShouldEqual($"+ $80.00");
        }

        [Fact]
        public void Converter_TransferSameAccount_Minus()
        {
            CultureHelper.CurrentCulture = new CultureInfo("en-US");
            var account = new AccountViewModel
                          {
                              Id = 4,
                              CurrentBalance = 400
                          };

            PaymentAmountConverterLogic.GetFormattedAmountString(new PaymentViewModel(mediatorMock.Object, navigationService.Object)
                                                                 {
                                                                     Amount = 80,
                                                                     Type = PaymentType.Transfer,
                                                                     ChargedAccountId = account.Id,
                                                                     ChargedAccount = account,
                                                                     CurrentAccountId = account.Id
                                                                 },
                                                                 string.Empty)
                                       .ShouldEqual("- $80.00");
            CultureHelper.CurrentCulture = CultureInfo.CurrentCulture;
        }

        [Fact]
        public void Converter_TransferSameAccount_Plus()
        {
            CultureHelper.CurrentCulture = new CultureInfo("en-US");
            var account = new AccountViewModel
                          {
                              Id = 4,
                              CurrentBalance = 400
                          };

            PaymentAmountConverterLogic.GetFormattedAmountString(new PaymentViewModel(mediatorMock.Object, navigationService.Object)
                                                                 {
                                                                     Amount = 80,
                                                                     Type = PaymentType.Transfer,
                                                                     ChargedAccount = new AccountViewModel(),
                                                                     CurrentAccountId = account.Id
                                                                 },
                                                                 string.Empty)
                                       .ShouldEqual("+ $80.00");
            CultureHelper.CurrentCulture = CultureInfo.CurrentCulture;
        }
    }
}
