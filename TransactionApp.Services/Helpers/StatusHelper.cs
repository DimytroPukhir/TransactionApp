namespace TransactionApp.Services.Helpers
{
    public static class StatusHelper
    {
        public static string GetUnifiedStatus(string transactionStatus)
        {
            string status = null;
            switch (transactionStatus.ToLower())
            {
                case "approved":
                    status = "A";
                    break;
                case "rejected":
                    status = "R";
                    break;
                case "failed":
                    status = "R";
                    break;
                case "done":
                    status = "D";
                    break;
                case "finished":
                    status = "F";
                    break;
            }

            return status;
        }
    }
}