using System;
using System.Collections.Generic;

namespace TransactionApp.Common.Exceptions
{
    public class InvalidFileException : Exception
    {
        public List<string> Errors { get; }

        public InvalidFileException(List<string> errors)
        {
            Errors = errors;
        }
    }
}