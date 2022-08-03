using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace clinic_assessment.data.Models.Response
{
    public class GenericResponse<T>
    {
        public int ResponseCode { get; set; }
        public T Message { get; set; }

        public GenericResponse()
        {
        }

        public GenericResponse(int responseCode, T message)
        {
            ResponseCode = responseCode;
            Message = message;
        }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
