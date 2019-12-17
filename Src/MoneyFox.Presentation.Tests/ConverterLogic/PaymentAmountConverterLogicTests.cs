using GalaSoft.MvvmLight.Views;
using MediatR;
using MoneyFox.Application.Common.ConverterLogic;
using MoneyFox.Domain;
using MoneyFox.Presentation.ViewModels;
using Moq;
using Should;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace MoneyFox.Presentation.Tests.ConverterLogic
{
    [ExcludeFromCodeCoverage]
    [Collection("MvxIocCollection")]
    public class PaymentAmountConverterLogicTests
    {
        readonly Mock<IMediator> mediatorMock;
        readonly Mock<INavigationService> navigationService;

        public PaymentAmountConverterLogicTests()
        {
            mediatorMock = new Mock<IMediator>();
            navigationService = new Mock<INavigationService>();
        }

        [Fact]
        public void Converter_Payment_NegativeAmountSign()
        {
            PaymentAmountConverterLogic.GetFormattedAmountString(new PaymentViewModel(mediatorMock.Object, navigationService.Object) { Amount = 80, Type = PaymentType.Expense },
                                                                 string.Empty)
                                       .ShouldEqual($"- {80:C}");
        }

        [Fact]
        public void Converter_Payment_PositiveAmountSign()
        {
            PaymentAmountConverterLogic.GetFormattedAmountString(new PaymentViewModel(mediatorMock.Object, navigationService.Object) { Amount = 80, Type = PaymentType.Income },
                                                                 string.Empty)
                                       .ShouldEqual($"+ {80:C}");
        }

        [Fact]
        public void Converter_TransferSameAccount_Minus()
        {
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
                                                                 }, string.Empty)
                                       .ShouldEqual($"- {80:C}");
        }

        [Fact]
        public void Converter_TransferSameAccount_Plus()
        {
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
                                                                 }, string.Empty)
                                       .ShouldEqual($"+ {80:C}");
        }
    }
}
