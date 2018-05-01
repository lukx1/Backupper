using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Objects.AdminExceptions
{
    public enum ErrorType
    {
        Unspecified,
        InvalidData,
        RequestedDataIsNull
    }

    public class AdminModelErrorException : AdminException
    {
        public AdminModelErrorException(string message) : base(message) { }

        public AdminModelErrorException(string message, ErrorType type) : base(message)
        {
            _errorType = type;
        }

        private ErrorType _errorType;
        public ErrorType ErrorType
        {
            get => _errorType;
        }
    }
}