using clinic_assessment_redone.Middleware.ErrorHandler.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace clinic_assessment.data.Models.Response
{
    public class SuccessResponse : GenericResponse<string>
    {
        public SuccessResponse() : base(StatusCodes.Status200OK, "")
        {
        }

        public SuccessResponse(string message) : base(StatusCodes.Status200OK, message)
        {
        }
    }
}
