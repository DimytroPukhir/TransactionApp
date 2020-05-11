using System.Xml.Serialization;
using System.Collections.Generic;

namespace TransactionApp.Services.Services.Transactions.Parsers.Models
{
    [XmlRoot(ElementName = "PaymentDetails")]
    public class PaymentDetails
    {
        [XmlElement(ElementName = "Amount")] public string Amount { get; set; }

        [XmlElement(ElementName = "CurrencyCode")]
        public string CurrencyCode { get; set; }
    }

    [XmlRoot(ElementName = "Transaction")]
    public class TransactionItem
    {
        [XmlElement(ElementName = "TransactionDate")]
        public string TransactionDate { get; set; }

        [XmlElement(ElementName = "PaymentDetails")]
        public PaymentDetails PaymentDetails { get; set; }

        [XmlElement(ElementName = "Status")] public string Status { get; set; }
        [XmlAttribute(AttributeName = "id")] public string PublicId { get; set; }
    }

    [XmlRoot(ElementName = "Transactions")]
    public class XmlTransactionsDto
    {
        [XmlElement(ElementName = "Transaction")]
        public List<TransactionItem> Transaction { get; set; }
    }
}