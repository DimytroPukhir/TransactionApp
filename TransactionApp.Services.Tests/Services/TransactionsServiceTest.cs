using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TransactionApp.DomainModel.Models;
using TransactionApp.Services.Abstractions;
using TransactionApp.Services.Infrastructure;
using TransactionApp.Services.Services;
using TransactionApp.Services.Services.Transactions.Abstractions;
using TransactionApp.Services.Services.Transactions.Parsers.Models;

namespace TransactionApp.Services.Tests.Services
{
    public class TransactionsServiceTest
    {
        private ITransactionsService _transactionsService;
        private Mock<ITransactionsDataParser> _transactionsDataParser;
        private Mock<ITransactionsParserProvider> _transactionsParserProviderMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        [SetUp]
        public void Init()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _transactionsParserProviderMock = new Mock<ITransactionsParserProvider>();
            _transactionsDataParser=new Mock<ITransactionsDataParser>();
            _transactionsService =
                new TransactionsService(_transactionsParserProviderMock.Object, _unitOfWorkMock.Object);
        }

        [TearDown]
        public void Dispose()
        {
            _unitOfWorkMock = null;
            _transactionsService = null;
        }

        [Test]
        public async Task AddTransactionsAsync_WithExistingItems_ReturnedItems()
        {
            //Arrange
            const string csvString = @"“Invoice0000001”,”700.00”,”EUR”,”21/02/2019 02:04:59”, “Done”";
            var csvDataStream = new MemoryStream(Encoding.UTF8.GetBytes(csvString));
            const string csvDatePattern = "dd/MM/yyyy hh:mm:ss";
            
            const string publicId = "Invoice0000001";
            const string code = "EUR";
            const string status = "D";
            const decimal amount = 700;
            var date = DateTimeOffset.ParseExact("21/02/2019 02:04:59",csvDatePattern ,CultureInfo.InvariantCulture);
            
            var model = new TransactionCreateModel
            {
                PublicId = publicId,
                Amount = amount,
                Date = date,
                Code = code,
                Status = status
            };
            var parseResults=new ParseResults(new List<TransactionCreateModel>{model});
            _transactionsParserProviderMock.Setup(x => x.GetParserFor(csvDataStream)).ReturnsAsync(_transactionsDataParser.Object);
            _transactionsDataParser.Setup(x => x.CanParseDataAsync(csvDataStream)).ReturnsAsync(true);
            _transactionsDataParser.Setup(x => x.ParseAllFileAsync()).ReturnsAsync(parseResults);
            _unitOfWorkMock.Setup(x => x.TransactionRepository.AddAsync(It.IsAny<TransactionCreateModel>()));
            
           
            //act
            await _transactionsService.AddTransactionsAsync(csvDataStream);

            //Assert
            _unitOfWorkMock.Verify(x => x.TransactionRepository.AddAsync(model), Times.Once);
        }
    }
}