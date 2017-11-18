using System;
using MoneyFox.Business.Extensions;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation;
using Xunit;

namespace MoneyFox.Service.Tests.Helpers
{
    public class RecurringPaymentHelperTests
    {
        [Theory]
        [InlineData(PaymentRecurrence.Daily, PaymentType.Income)]
        [InlineData(PaymentRecurrence.DailyWithoutWeekend, PaymentType.Income)]
        [InlineData(PaymentRecurrence.Weekly, PaymentType.Income)]
        [InlineData(PaymentRecurrence.Biweekly, PaymentType.Income)]
        [InlineData(PaymentRecurrence.Monthly, PaymentType.Income)]
        [InlineData(PaymentRecurrence.Bimonthly, PaymentType.Income)]
        [InlineData(PaymentRecurrence.Quarterly, PaymentType.Income)]
        [InlineData(PaymentRecurrence.Biannually, PaymentType.Income)]
        [InlineData(PaymentRecurrence.Yearly, PaymentType.Income)]
        [InlineData(PaymentRecurrence.Daily, PaymentType.Expense)]
        [InlineData(PaymentRecurrence.DailyWithoutWeekend, PaymentType.Expense)]
        [InlineData(PaymentRecurrence.Weekly, PaymentType.Expense)]
        [InlineData(PaymentRecurrence.Biweekly, PaymentType.Expense)]
        [InlineData(PaymentRecurrence.Monthly, PaymentType.Expense)]
        [InlineData(PaymentRecurrence.Bimonthly, PaymentType.Expense)]
        [InlineData(PaymentRecurrence.Quarterly, PaymentType.Expense)]
        [InlineData(PaymentRecurrence.Biannually, PaymentType.Expense)]
        [InlineData(PaymentRecurrence.Yearly, PaymentType.Expense)]
        [InlineData(PaymentRecurrence.Daily, PaymentType.Transfer)]
        [InlineData(PaymentRecurrence.DailyWithoutWeekend, PaymentType.Transfer)]
        [InlineData(PaymentRecurrence.Weekly, PaymentType.Transfer)]
        [InlineData(PaymentRecurrence.Biweekly, PaymentType.Transfer)]
        [InlineData(PaymentRecurrence.Monthly, PaymentType.Transfer)]
        [InlineData(PaymentRecurrence.Bimonthly, PaymentType.Transfer)]
        [InlineData(PaymentRecurrence.Quarterly, PaymentType.Transfer)]
        [InlineData(PaymentRecurrence.Biannually, PaymentType.Transfer)]
        [InlineData(PaymentRecurrence.Yearly, PaymentType.Transfer)]
        public void GetRecurringFromPayment_Endless(PaymentRecurrence recurrence, PaymentType type)
        {
            // Arrange
            var startDate = new DateTime(2015, 03, 12);
            var enddate = Convert.ToDateTime("7.8.2016");

            var payment = new Payment
            {
                Data =
                {
                    ChargedAccount = new AccountEntity {Id = 3},
                    TargetAccount = new AccountEntity {Id = 8},
                    Category = new CategoryEntity {Id = 16},
                    Date = startDate,
                    Amount = 2135,
                    IsCleared = false,
                    Type = type,
                    IsRecurring = true
                }
            };

            // Act
            var recurring = RecurringPaymentHelper.GetRecurringFromPayment(payment, true,
                                                                           recurrence, enddate);

            recurring.Data.ChargedAccount.Id.ShouldBe(3);
            recurring.Data.TargetAccount.Id.ShouldBe(8);
            recurring.Data.StartDate.ShouldBe(startDate);
            recurring.Data.EndDate.ShouldBeNull();
            recurring.Data.IsEndless.ShouldBe(true);
            recurring.Data.Amount.ShouldBe(payment.Data.Amount);
            recurring.Data.Category.Id.ShouldBe(payment.Data.Category.Id);
            recurring.Data.Type.ShouldBe(type);
            recurring.Data.Recurrence.ShouldBe(recurrence);
            recurring.Data.Note.ShouldBe(payment.Data.Note);
        }


        [Theory]
        [InlineData(PaymentRecurrence.Daily)]
        [InlineData(PaymentRecurrence.DailyWithoutWeekend)]
        [InlineData(PaymentRecurrence.Weekly)]
        [InlineData(PaymentRecurrence.Biweekly)]
        [InlineData(PaymentRecurrence.Monthly)]
        [InlineData(PaymentRecurrence.Bimonthly)]
        [InlineData(PaymentRecurrence.Quarterly)]
        [InlineData(PaymentRecurrence.Biannually)]
        [InlineData(PaymentRecurrence.Yearly)]
        public void GetPaymentFromRecurring_CorrectMappedPayment(PaymentRecurrence recurrence)
        {
            var account = new AccountEntity {Id = 2};

            var recurringPayment = new RecurringPayment
            {
                Data =
                {
                    Id = 4,
                    Recurrence = recurrence,
                    StartDate = new DateTime(2015, 08, DateTime.Today.Day),
                    ChargedAccountId = 2,
                    ChargedAccount = account,
                    Amount = 105
                }
            };

            var result = RecurringPaymentHelper.GetPaymentFromRecurring(recurringPayment);

            result.Data.ChargedAccount.ShouldBe(account);
            result.Data.ChargedAccountId.ShouldBe(account.Id);
            result.Data.Amount.ShouldBe(105);
            result.Data.Date.ShouldBe(DateTime.Today);
        }

        [Fact]
        public void GetPaymentFromRecurring_MonthlyPayment_CorrectMappedPayment()
        {
            var account = new AccountEntity {Id = 2};
            var dayOfMonth = 26;

            var recurringPayment = new RecurringPayment
            {
                Data =
                {
                    Id = 4,
                    Recurrence = PaymentRecurrence.Monthly,
                    StartDate = new DateTime(2015, 08, dayOfMonth),
                    ChargedAccountId = 2,
                    ChargedAccount = account,
                    Amount = 105
                }
            };

            var result = RecurringPaymentHelper.GetPaymentFromRecurring(recurringPayment);

            result.Data.ChargedAccount.ShouldBe(account);
            result.Data.ChargedAccountId.ShouldBe(account.Id);
            result.Data.Amount.ShouldBe(105);
            result.Data.Date.ShouldBe(new DateTime(DateTime.Today.Year, DateTime.Today.Month, dayOfMonth));
        }

        [Fact]
        public void GetRecurringFromPayment_IsEndless_EnddateNull()
        {
            // Arrange
            var payment = new Payment
            {
                Data = new PaymentEntity
                {
                    Amount = 12,
                    Category = new CategoryEntity { Name = "category" },
                    ChargedAccount = new AccountEntity { Name = "account" },
                    Date = DateTime.Now,
                    IsCleared = false,
                    Note = "note"
                }
            };

            // Act
            var recurringPayment = RecurringPaymentHelper.GetRecurringFromPayment(payment, true, PaymentRecurrence.Daily);

            // Assert
            Assert.Null(recurringPayment.Data.EndDate);
        }

        [Fact]
        public void GetRecurringFromPayment_IsNotEndless_DefaultEnddate()
        {
            // Arrange
            var payment = new Payment
            {
                Data = new PaymentEntity
                {
                    Amount = 12,
                    Category = new CategoryEntity { Name = "category" },
                    ChargedAccount = new AccountEntity { Name = "account" },
                    Date = DateTime.Now,
                    IsCleared = false,
                    Note = "note"
                }
            };

            // Act
            var recurringPayment = RecurringPaymentHelper.GetRecurringFromPayment(payment, false, PaymentRecurrence.Daily);

            // Assert
            Assert.NotNull(recurringPayment.Data.EndDate);
            Assert.Equal(new DateTime(), recurringPayment.Data.EndDate.Value);
        }

        [Fact]
        public void GetRecurringFromPayment_IsNotEndless_EnddateSet()
        {
            // Arrange
            var payment = new Payment
            {
                Data = new PaymentEntity
                {
                    Amount = 12,
                    Category = new CategoryEntity { Name = "category" },
                    ChargedAccount = new AccountEntity { Name = "account" },
                    Date = DateTime.Now,
                    IsCleared = false,
                    Note = "note"
                }
            };
            var enddate = DateTime.Now.AddMonths(2);

            // Act
            var recurringPayment = RecurringPaymentHelper.GetRecurringFromPayment(payment, false, PaymentRecurrence.Daily, enddate);

            // Assert
            Assert.NotNull(recurringPayment.Data.EndDate);
            Assert.Equal(enddate, recurringPayment.Data.EndDate.Value);
        }

        [Theory]
        [InlineData(PaymentRecurrence.Daily, 1, true)]
        [InlineData(PaymentRecurrence.Weekly, 8, true)]
        [InlineData(PaymentRecurrence.Biweekly, 14, true)]
        [InlineData(PaymentRecurrence.Monthly, 31, true)]
        [InlineData(PaymentRecurrence.Bimonthly, 62, true)]
        [InlineData(PaymentRecurrence.Quarterly, 94, true)]
        [InlineData(PaymentRecurrence.Biannually, 184, true)]
        [InlineData(PaymentRecurrence.Yearly, 365, true)]
        [InlineData(PaymentRecurrence.Daily, 0, false)]
        [InlineData(PaymentRecurrence.Weekly, 5, false)]
        [InlineData(PaymentRecurrence.Biweekly, 10, false)]
        [InlineData(PaymentRecurrence.Bimonthly, 20, false)]
        [InlineData(PaymentRecurrence.Quarterly, 59, false)]
        [InlineData(PaymentRecurrence.Biannually, 137, false)]
        [InlineData(PaymentRecurrence.Yearly, 300, false)]
        [InlineData(PaymentRecurrence.Biannually, 355, true)] // with year change
        [InlineData(PaymentRecurrence.Quarterly, 355, true)] // with year change
        public void CheckIfRepeatable_ValidatedRecurrence(PaymentRecurrence recurrence, int amountOfDaysPassed,
                                                          bool expectedResult)
        {
            var account = new AccountEntity {Id = 2};

            var recurringPayment = new RecurringPaymentEntity
            {
                Id = 4,
                Recurrence = recurrence,
                StartDate = new DateTime(2015, 08, 25),
                ChargedAccountId = 2,
                ChargedAccount = account,
                Amount = 105
            };

            RecurringPaymentHelper.CheckIfRepeatable(
                                      new Payment
                                      {
                                          Data =
                                          {
                                              Date = DateTime.Today.AddDays(-amountOfDaysPassed),
                                              IsCleared = true,
                                              RecurringPayment = recurringPayment
                                          }
                                      })
                                  .ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData(PaymentRecurrence.Daily, 0)]
        [InlineData(PaymentRecurrence.Weekly, 5)]
        [InlineData(PaymentRecurrence.Biweekly, 10)]
        [InlineData(PaymentRecurrence.Monthly, 28)]
        [InlineData(PaymentRecurrence.Bimonthly, 55)]
        [InlineData(PaymentRecurrence.Yearly, 340)]
        public void CheckIfRepeatable_UnclearedPayment_ReturnFalse(
            PaymentRecurrence recurrence, int amountOfDaysUntilRepeat)
        {
            var account = new AccountEntity {Id = 2};

            var recurringPayment = new RecurringPaymentEntity
            {
                Id = 4,
                Recurrence = PaymentRecurrence.Weekly,
                StartDate = new DateTime(2015, 08, 25),
                ChargedAccountId = 2,
                ChargedAccount = account,
                Amount = 105
            };

            RecurringPaymentHelper.CheckIfRepeatable(new Payment
                                  {
                                      Data =
                                      {
                                          Date = DateTime.Today.AddDays(amountOfDaysUntilRepeat),
                                          RecurringPayment = recurringPayment
                                      }
                                  })
                                  .ShouldBeFalse();
        }

        [Fact]
        public void CheckIfRepeatable_ValidatedRecurrenceMonthly_False()
        {
            var account = new AccountEntity {Id = 2};

            var recurringPayment = new RecurringPaymentEntity
            {
                Id = 4,
                Recurrence = PaymentRecurrence.Monthly,
                StartDate = new DateTime(2015, 08, 25),
                ChargedAccountId = 2,
                ChargedAccount = account,
                Amount = 105
            };

            RecurringPaymentHelper.CheckIfRepeatable(new Payment
                                  {
                                      Data =
                                      {
                                          Date = DateTime.Today.GetFirstDayOfMonth(),
                                          IsCleared = true,
                                          RecurringPayment = recurringPayment
                                      }
                                  })
                                  .ShouldBeFalse();
        }
    }
}