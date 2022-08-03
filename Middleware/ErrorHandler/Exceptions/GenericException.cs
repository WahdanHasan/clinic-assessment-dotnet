using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace clinic_assessment_redone.Middleware.ErrorHandler.Exceptions
{
    public class GenericException<T> : Exception
    {
        public int ResponseCode { get; set; }

        public T Message { get; set; }

        public GenericException(int responseCode, T message)
        {
            ResponseCode = responseCode;
            Message = message;
        }


    }
}
