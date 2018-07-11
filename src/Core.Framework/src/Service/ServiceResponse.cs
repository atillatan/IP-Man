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
    [DataContract]
    public class ServiceResponse<T>
    {
        [DataMember]
        public bool IsSuccess { get; set; }

        [DataMember]
        public ResultType ResultType { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public int TotalCount { get; set; }

        [DataMember]
        public T Data { get; set; }

        private ServiceResponse()
        {
        }

        public ServiceResponse(T Data) : this(true, ResultType.Success, string.Empty, Data, 0)
        {
        }

        public ServiceResponse(T Data, int TotalCount) : this(true, ResultType.Success, string.Empty, Data, TotalCount)
        {
        }

        public ServiceResponse(T Data, string Message) : this(true, ResultType.Success, Message, Data, 0)
        {
        }

        public ServiceResponse(ResultType ResultType, string Message) : this(true, ResultType, Message, default(T), 0)
        {
        }
        public ServiceResponse(bool IsSuccess, ResultType ResultType, string Message) : this(IsSuccess, ResultType, Message, default(T), 0)
        {
        }

        public ServiceResponse(bool IsSuccess, ResultType ResultType, string Message, T Data) : this(IsSuccess, ResultType, Message, Data, 0)
        {
        }

        public ServiceResponse(bool IsSuccess, ResultType ResultType, string Message, T Data, int TotalCount)
        {
            this.IsSuccess = IsSuccess;
            this.ResultType = ResultType;
            this.Message = Message;
            this.Data = Data;
            this.TotalCount = TotalCount;
        }
    }

    public enum ResultType : int
    {
        Information = 1,
        Validation = 2,
        Success = 3,
        Warning = 4,
        Error = 5
    };
}
