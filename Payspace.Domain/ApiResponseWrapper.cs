using System;
using System.Collections.Generic;
using System.Text;

namespace Payspace.Domain
{
    public class ApiResponseWrapper<T>
    {
        public ApiResponseWrapper(T payload, string errorMessage=null)
        {
            Payload = payload;
            ErrorMessage = errorMessage;
        }
        public T Payload { get; }
        public string ErrorMessage { get; }
    }
}
