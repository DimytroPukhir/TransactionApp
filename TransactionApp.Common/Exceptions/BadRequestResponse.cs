using System;
using System.Collections.Generic;

namespace TransactionApp.Common.Exceptions
{
    public class BadRequestResponse:Exception
    {
        public BadRequestResponse(Dictionary<int, List<string>> errors)
        {
            Errors = errors;
        }

        public Dictionary<int,List<string>>Errors { get; }
        
    }
}