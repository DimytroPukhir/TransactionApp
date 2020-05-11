using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using TransactionApp.DomainModel.Models;
using TransactionApp.Mappers;
using TransactionApp.Models;

namespace TransactionApp.Tests.Mappers
{
    public class TransactionViewModelMapperTest
    {
        private Fixture _fixture;
        private TransactionViewModelMapper _transactionViewModelMapper;

        [SetUp]
        public void Init()
        {
            _fixture = new Fixture();
            _transactionViewModelMapper = new TransactionViewModelMapper();
        }

        [TearDown]
        public void Dispose()
        {
            _fixture = null;
            _transactionViewModelMapper = null;
        }
        [Test]
        public void Map_AllProperties_MappedCorrectly()
        {
            //Arrange
            var transaction = _fixture.Create<Transaction>();

            var expected = new TransactionViewModel(transaction.PublicId,
                $"{transaction.Amount}{transaction.Code}",
                transaction.Status);


            //Act
            var actual = _transactionViewModelMapper.Map(transaction);

            //Assert
            actual.Should().BeEquivalentTo(expected);
        }
    }
}