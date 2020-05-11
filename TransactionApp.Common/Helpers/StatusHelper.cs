using System;
using System.Collections.Generic;
using System.Linq;

namespace TransactionApp.Common.Helpers
{
    public static class StatusHelper
    {
        public static List<string> Statuses = new List<string> {"F", "D", "R", "A"};

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

        public static bool IsAppropriateStatus(string status)
        {
            return Statuses.Any(x => x.Equals(status, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}