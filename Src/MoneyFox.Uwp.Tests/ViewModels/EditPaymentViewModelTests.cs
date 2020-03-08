using AutoMapper;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetAccounts;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Payments.Commands.UpdatePayment;
using MoneyFox.Application.Payments.Queries.GetPaymentById;
using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.Tests.Collections;
using MoneyFox.Uwp.ViewModels;
using Moq;
using Should;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Windows.Globalization;
using Xunit;

namespace MoneyFox.Uwp.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    [Collection("AutoMapperCollection")]
    public class EditPaymentViewModelTests
    {
        private readonly IMapper mapper;
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IDialogService> dialogServiceMock;
        private readonly Mock<INavigationService> navigationServiceMock;

        public EditPaymentViewModelTests(MapperCollectionFixture fixture)
        {
            mediatorMock = new Mock<IMediator>();
            mapper = fixture.Mapper;

            dialogServiceMock = new Mock<IDialogService>();
            navigationServiceMock = new Mock<INavigationService>();

            mediatorMock.Setup(x => x.Send(It.IsAny<GetAccountsQuery>(), default))
                        .ReturnsAsync(new List<Account>());
            mediatorMock.Setup(x => x.Send(It.IsAny<UpdatePaymentCommand>(), default))
                        .ReturnsAsync(Unit.Value);
            mediatorMock.Setup(x => x.Send(It.IsAny<GetPaymentByIdQuery>(), default))
                        .ReturnsAsync(new Payment(DateTime.Now, 12.10M, PaymentType.Expense, new Account("sad")));
        }

        [Fact]
        public async Task AmountStringSetOnInit()
        {
            // Arrange
            var cultureString = "en-Gb";
            CultureInfo ci = new CultureInfo(cultureString);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            ApplicationLanguages.PrimaryLanguageOverride = cultureString;

            mediatorMock.Setup(x => x.Send(It.IsAny<GetPaymentByIdQuery>(), default))
                        .ReturnsAsync(new Payment(DateTime.Now, 12.10M, PaymentType.Expense, new Account("sad")));

            var editPaymentVm = new EditPaymentViewModel(mediatorMock.Object,
                                                         mapper,
                                                         dialogServiceMock.Object,
                                                         navigationServiceMock.Object);

            // Act
            await editPaymentVm.InitializeCommand.ExecuteAsync();

            // Assert
            editPaymentVm.AmountString.ShouldEqual("12.10");
        }

        [Theory]
        [InlineData("de-CH", "12.20", 12.20)]
        [InlineData("de-DE", "12,20", 12.20)]
        [InlineData("en-US", "12.20", 12.20)]
        [InlineData("ru-RU", "12,20", 12.20)]
        [InlineData("de-CH", "-12.20", -12.20)]
        [InlineData("de-DE", "-12,20", -12.20)]
        [InlineData("en-US", "-12.20", -12.20)]
        [InlineData("ru-RU", "-12,20", -12.20)]
        public async Task AmountCorrectlyFormattedOnSave(string cultureString, string amountString, decimal expectedAmount)
        {
            // Arrange
            CultureInfo ci = new CultureInfo(cultureString);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            ApplicationLanguages.PrimaryLanguageOverride = cultureString;
            ApplicationLanguages.PrimaryLanguageOverride = cultureString;

            var editPaymentVm = new EditPaymentViewModel(mediatorMock.Object,
                                                         mapper,
                                                         dialogServiceMock.Object,
                                                         navigationServiceMock.Object);

            await editPaymentVm.InitializeCommand.ExecuteAsync();
            editPaymentVm.SelectedPayment.ChargedAccount = new AccountViewModel { Name = "asdf" };

            // Act
            editPaymentVm.AmountString = amountString;
            await editPaymentVm.SaveCommand.ExecuteAsync();

            // Assert
            editPaymentVm.SelectedPayment.Amount.ShouldEqual(expectedAmount);
        }

        [Fact]
        public async Task ShowMessageIfAmountIsNegativeOnSave()
        {
            // Arrange
            var editPaymentVm = new EditPaymentViewModel(mediatorMock.Object,
                                                         mapper,
                                                         dialogServiceMock.Object,
                                                         navigationServiceMock.Object);

            await editPaymentVm.InitializeCommand.ExecuteAsync();
            editPaymentVm.AmountString = "-2";

            // Act
            await editPaymentVm.SaveCommand.ExecuteAsync();

            // Assert
            dialogServiceMock.Verify(x => x.ShowMessageAsync(Strings.AmountMayNotBeNegativeTitle, Strings.AmountMayNotBeNegativeMessage),
                                     Times.Once);
            navigationServiceMock.Verify(x => x.GoBack(), Times.Never);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("2")]
        public async Task ShowNoMessageIfAmountIsPositiveOnSave(string amountString)
        {
            // Arrange
            var editPaymentVm = new EditPaymentViewModel(mediatorMock.Object,
                                                         mapper,
                                                         dialogServiceMock.Object,
                                                         navigationServiceMock.Object);

            await editPaymentVm.InitializeCommand.ExecuteAsync();
            editPaymentVm.AmountString = amountString;

            // Act
            await editPaymentVm.SaveCommand.ExecuteAsync();

            // Assert
            dialogServiceMock.Verify(x => x.ShowMessageAsync(Strings.AmountMayNotBeNegativeTitle, Strings.AmountMayNotBeNegativeMessage),
                                     Times.Never);
        }
    }
}
