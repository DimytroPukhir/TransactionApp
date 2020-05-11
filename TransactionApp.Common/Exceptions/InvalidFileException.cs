using System;
using System.Collections.Generic;

namespace TransactionApp.Common.Exceptions
{
    public class InvalidFileException:Exception
    {
        public InvalidFileException(List<string> errors)
        {
            Errors = errors;
        }
        
        public List<string> Errors { get; }
    }
}