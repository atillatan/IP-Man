/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 10:10:45 
 */
using System;
using System.Runtime.Serialization;

namespace Core.Framework.Service
{
    [Serializable]
    public class ServiceException : Exception
    {
        public ExceptionType ExceptionType { get; private set; }
        public ServiceException() : this(ExceptionType.Error, string.Empty, null)
        { }

        public ServiceException(string message) : this(ExceptionType.Error, message, null)
        { }
        public ServiceException(ExceptionType exceptionType) : this(exceptionType, string.Empty, null)
        { }

        public ServiceException(ExceptionType exceptionType, string message) : this(exceptionType, message, null)
        { }

        public ServiceException(string message, Exception innerException) : this(ExceptionType.Error, message, innerException)
        { }
        public ServiceException(ExceptionType exceptionType, string message, Exception innerException) : base(message, innerException)
        {
            this.ExceptionType = exceptionType;
        }
        protected ServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.ExceptionType = ExceptionType.Error;
        }
    }

    public enum ExceptionType : int
    {
        Information = 1,
        Warning = 4,
        Error = 5
    };
}