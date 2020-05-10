namespace TransactionApp.Models
{
    public class TransactionViewModel
    {
        public TransactionViewModel()
        {
        }

        public TransactionViewModel( string publicId, string payment, string status)
        {
            PublicId = publicId;
            Payment = payment;
            Status = status;
        }

        public string PublicId { get; }
        public string Payment { get; }
        public string Status { get; }
    }
}