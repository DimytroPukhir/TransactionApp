using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TransactionApp.DomainModel.Models;
using TransactionApp.Services.Abstractions;
using TransactionApp.Services.Infrastructure;
using TransactionApp.Services.Providers;

namespace TransactionApp.Services.Tests.Providers
{
    public class TransactionsProviderTest
    {
        private Fixture _fixture;
        private ITransactionsProvider _transactionsProvider;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        [SetUp]
        public void Init()
        {
            _fixture = new Fixture();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _transactionsProvider = new TransactionsProvider(_unitOfWorkMock.Object);
        }

        [TearDown]
        public void Dispose()
        {
            _fixture = null;
            _unitOfWorkMock = null;
            _transactionsProvider = null;
        }

        [Test]
        public async Task GetFilteredAsync_WithExistingItems_ReturnedItems()
        {
            //Arrange
            var startDate = new DateTimeOffset();
            var endDate = new DateTimeOffset();
            const string status = "Status";
            const string currencyCode = "CurrencyCode";
            var expected = _fixture.Create<List<Transaction>>();

            _unitOfWorkMock.Setup(r =>
                    r.TransactionRepository.GetFiltered(currencyCode,
                        null,
                        null,
                        status))
                .ReturnsAsync(expected);


            //Act
            var actual = await _transactionsProvider.GetFilteredAsync(
                currencyCode,
                null,
                null,
                status);

            //Assert
            actual.Should().BeEquivalentTo(expected);
            _unitOfWorkMock.VerifyAll();
        }
    }
}