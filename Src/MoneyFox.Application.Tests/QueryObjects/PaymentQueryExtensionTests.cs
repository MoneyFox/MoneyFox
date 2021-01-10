using FluentAssertions;
using MoneyFox.Application.Common.QueryObjects;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace MoneyFox.Application.Tests.QueryObjects
{
    [ExcludeFromCodeCoverage]
    public class PaymentQueryExtensionTests
    {
        [Fact]
        public void HasNotId()
        {
            // Arrange
            IQueryable<Payment> paymentListQuery = new List<Payment>
            {
                new Payment(DateTime.Now, 12, PaymentType.Expense, new Account("d")),
                new Payment(DateTime.Now.AddDays(1), 13, PaymentType.Expense, new Account("d")),
                new Payment(DateTime.Now.AddDays(1), 14, PaymentType.Expense, new Account("d"))
            }.AsQueryable();

            // Act
            var resultList = paymentListQuery.HasNotId(paymentListQuery.First().Id).ToList();

            // Assert
            // Since we haven't added the payments to the DB all Id's are 0.
            resultList.Should().BeEmpty();
        }

        [Fact]
        public void AreAfterOrEqual()
        {
            // Arrange
            IQueryable<Payment> paymentListQuery = new List<Payment>
            {
                new Payment(DateTime.Now.AddDays(1), 12, PaymentType.Expense, new Account("d")),
                new Payment(DateTime.Now, 13, PaymentType.Expense, new Account("d")),
                new Payment(DateTime.Now.AddDays(-1), 14, PaymentType.Expense, new Account("d"))
            }.AsQueryable();

            // Act
            var resultList = paymentListQuery.AreAfterOrEqual(DateTime.Now).ToList();

            // Assert
            Assert.Equal(2, resultList.Count);
            Assert.Equal(12, resultList[0].Amount);
            Assert.Equal(13, resultList[1].Amount);
        }

        [Fact]
        public void IsCleared()
        {
            // Arrange
            IQueryable<Payment> paymentListQuery = new List<Payment>
            {
                new Payment(DateTime.Now, 12, PaymentType.Expense, new Account("d")),
                new Payment(DateTime.Now.AddDays(1), 13, PaymentType.Expense, new Account("d")),
                new Payment(DateTime.Now.AddDays(1), 14, PaymentType.Expense, new Account("d"))
            }.AsQueryable();

            // Act
            var resultList = paymentListQuery.AreCleared().ToList();

            // Assert
            Assert.Single(resultList);
            Assert.Equal(12, resultList[0].Amount);
        }

        [Fact]
        public void IsNotCleared()
        {
            // Arrange
            IQueryable<Payment> paymentListQuery = new List<Payment>
            {
                new Payment(DateTime.Now, 12, PaymentType.Expense, new Account("d")),
                new Payment(DateTime.Now.AddDays(1), 13, PaymentType.Expense, new Account("d")),
                new Payment(DateTime.Now.AddDays(1), 14, PaymentType.Expense, new Account("d"))
            }.AsQueryable();

            // Act
            var resultList = paymentListQuery.AreNotCleared().ToList();

            // Assert
            Assert.Equal(2, resultList.Count);
            Assert.Equal(13, resultList[0].Amount);
            Assert.Equal(14, resultList[1].Amount);
        }

        [Fact]
        public void WithoutTransfers()
        {
            // Arrange
            IQueryable<Payment> paymentListQuery = new List<Payment>
            {
                new Payment(DateTime.Now,
                            12,
                            PaymentType.Expense,
                            new Account("d"),
                            new Account("t")),
                new Payment(DateTime.Now,
                            13,
                            PaymentType.Income,
                            new Account("d"),
                            new Account("t")),
                new Payment(DateTime.Now,
                            14,
                            PaymentType.Transfer,
                            new Account("d"),
                            new Account("t")),
                new Payment(DateTime.Now,
                            15,
                            PaymentType.Income,
                            new Account("d"),
                            new Account("t")),
                new Payment(DateTime.Now,
                            16,
                            PaymentType.Transfer,
                            new Account("d"),
                            new Account("t"))
            }.AsQueryable();

            // Act
            var resultList = paymentListQuery.WithoutTransfers().ToList();

            // Assert
            Assert.Equal(3, resultList.Count);
            Assert.Equal(12, resultList[0].Amount);
            Assert.Equal(13, resultList[1].Amount);
            Assert.Equal(15, resultList[2].Amount);
        }
    }
}
