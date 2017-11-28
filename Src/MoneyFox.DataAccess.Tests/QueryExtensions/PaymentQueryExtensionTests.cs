using System;
using System.Collections.Generic;
using System.Linq;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.QueryExtensions;
using MoneyFox.Foundation;
using Xunit;

namespace MoneyFox.DataAccess.Tests.QueryExtensions
{
    public class PaymentQueryExtensionTests
    {
        [Fact]
        public void IsNotCleared()
        {
            // Arrange
            var paymentListQuery = new List<PaymentEntity>
                {
                    new PaymentEntity {Id = 1, IsCleared = true},
                    new PaymentEntity {Id = 2, IsCleared = false},
                    new PaymentEntity {Id = 3, IsCleared = false}
                }
                .AsQueryable();

            // Act
            var resultList = paymentListQuery.AreNotCleared().ToList();

            // Assert
            Assert.Equal(2, resultList.Count);
            Assert.Equal(2, resultList[0].Id);
            Assert.Equal(3, resultList[1].Id);
        }

        [Fact]
        public void IsCleared()
        {
            // Arrange
            var paymentListQuery = new List<PaymentEntity>
                {
                    new PaymentEntity {Id = 1, IsCleared = true},
                    new PaymentEntity {Id = 2, IsCleared = false},
                    new PaymentEntity {Id = 3, IsCleared = false}
                }
                .AsQueryable();

            // Act
            var resultList = paymentListQuery.AreCleared().ToList();

            // Assert
            Assert.Equal(1, resultList.Count);
            Assert.Equal(1, resultList[0].Id);
        }

        [Fact]
        public void HasDateLargerEqualsThan()
        {
            // Arrange
            var date = DateTime.Now;
            var paymentListQuery = new List<PaymentEntity>
                {
                    new PaymentEntity {Id = 1, Date = date.AddDays(-1)},
                    new PaymentEntity {Id = 2, Date = date},
                    new PaymentEntity {Id = 3, Date = date.AddDays(1)}
                }
                .AsQueryable();

            // Act
            var resultList = paymentListQuery.HasDateLargerEqualsThan(date).ToList();

            // Assert
            Assert.Equal(2, resultList.Count);
            Assert.Equal(2, resultList[0].Id);
            Assert.Equal(3, resultList[1].Id);
        }

        [Fact]
        public void HasDateSmallerEqualsThan()
        {
            // Arrange
            var date = DateTime.Now;
            var paymentListQuery = new List<PaymentEntity>
                {
                    new PaymentEntity {Id = 1, Date = date.AddDays(-1)},
                    new PaymentEntity {Id = 2, Date = date},
                    new PaymentEntity {Id = 3, Date = date.AddDays(1)}
                }
                .AsQueryable();

            // Act
            var resultList = paymentListQuery.HasDateSmallerEqualsThan(date).ToList();

            // Assert
            Assert.Equal(2, resultList.Count);
            Assert.Equal(1, resultList[0].Id);
            Assert.Equal(2, resultList[1].Id);
        }

        [Fact]
        public void HasAccountId()
        {
            // Arrange
            var paymentListQuery = new List<PaymentEntity>
                {
                    new PaymentEntity {Id = 1, ChargedAccountId = 4, TargetAccountId = 0},
                    new PaymentEntity {Id = 2, ChargedAccountId = 4, TargetAccountId = 5},
                    new PaymentEntity {Id = 3, ChargedAccountId = 1, TargetAccountId = 0},
                    new PaymentEntity {Id = 4, ChargedAccountId = 3, TargetAccountId = 1},

                }
                .AsQueryable();

            // Act
            var resultList = paymentListQuery.HasAccountId(1).ToList();

            // Assert
            Assert.Equal(2, resultList.Count);
            Assert.Equal(3, resultList[0].Id);
            Assert.Equal(4, resultList[1].Id);
        }

        [Fact]
        public void HasChargedAccountId()
        {
            // Arrange
            var paymentListQuery = new List<PaymentEntity>
                {
                    new PaymentEntity {Id = 1, ChargedAccountId = 4, TargetAccountId = 0},
                    new PaymentEntity {Id = 2, ChargedAccountId = 4, TargetAccountId = 5},
                    new PaymentEntity {Id = 3, ChargedAccountId = 1, TargetAccountId = 0},
                    new PaymentEntity {Id = 4, ChargedAccountId = 3, TargetAccountId = 1},

                }
                .AsQueryable();

            // Act
            var resultList = paymentListQuery.HasChargedAccountId(1).ToList();

            // Assert
            Assert.Equal(1, resultList.Count);
            Assert.Equal(3, resultList[0].Id);
        }

        [Fact]
        public void HasTargetAccountId()
        {
            // Arrange
            var paymentListQuery = new List<PaymentEntity>
                {
                    new PaymentEntity {Id = 1, ChargedAccountId = 4, TargetAccountId = 0},
                    new PaymentEntity {Id = 2, ChargedAccountId = 4, TargetAccountId = 5},
                    new PaymentEntity {Id = 3, ChargedAccountId = 1, TargetAccountId = 0},
                    new PaymentEntity {Id = 4, ChargedAccountId = 3, TargetAccountId = 1},

                }
                .AsQueryable();

            // Act
            var resultList = paymentListQuery.HasTargetAccountId(1).ToList();

            // Assert
            Assert.Equal(1, resultList.Count);
            Assert.Equal(4, resultList[0].Id);
        }

        [Fact]
        public void WithoutTransfers()
        {
            // Arrange
            var paymentListQuery = new List<PaymentEntity>
                {
                    new PaymentEntity {Id = 1, Type = PaymentType.Expense},
                    new PaymentEntity {Id = 2, Type = PaymentType.Income},
                    new PaymentEntity {Id = 3, Type = PaymentType.Transfer},
                    new PaymentEntity {Id = 4, Type = PaymentType.Income},
                    new PaymentEntity {Id = 5, Type = PaymentType.Transfer},
                }
                .AsQueryable();

            // Act
            var resultList = paymentListQuery.WithoutTransfers().ToList();

            // Assert
            Assert.Equal(3, resultList.Count);
            Assert.Equal(1, resultList[0].Id);
            Assert.Equal(2, resultList[1].Id);
            Assert.Equal(4, resultList[2].Id);
        }

        [Fact]
        public void SelectPayments()
        {
            // Arrange
            var paymentListQuery = new List<PaymentEntity>
                {
                    new PaymentEntity {Id = 1},
                    new PaymentEntity {Id = 2}
                }
                .AsQueryable();

            // Act
            var resultList = paymentListQuery.SelectPayments().ToList();

            // Assert
            Assert.Equal(2, resultList.Count);
            Assert.Equal(1, resultList[0].Data.Id);
            Assert.Equal(2, resultList[1].Data.Id);
        }
    }
}
