using System;

namespace FscmBridgeServices.Exceptions
{

    public class InternalErrorException : Exception
    {
        public InternalErrorException() : base("An internal error occurred.")
        {
        }

        public InternalErrorException(string message) : base(message)
        {
        }

        public InternalErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public override string Message => base.Message + " Please contact support if the issue persists.";
    }
}