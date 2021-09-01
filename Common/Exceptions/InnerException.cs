using System;

namespace Common.Exceptions
{
    public class InnerException : Exception
    {
        public InnerException(string message, string exceptionCode, string propertyName = "") : base(message)
        {
            ExceptionCode = exceptionCode;
            PropertyName = propertyName;
        }
        public string PropertyName { get; set; }

        public string ExceptionCode { get; set; }
    }
}
