using clinic_assessment.data.Models.Response;
using clinic_assessment_redone.Helpers.Misc;
using clinic_assessment_redone.Middleware.ErrorHandler.Exceptions;

namespace clinic_assessment_redone.Middleware.ErrorHandler
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (RestException re)
            {
                await HandleExceptionAsync(httpContext, re.ResponseCode, re.Message);
            }
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex.StackTrace);
            //    await HandleExceptionAsync(httpContext, StatusCodes.Status500InternalServerError, Constants.INTERNAL_SERVER_ERROR);
            //}
        }
        private async Task HandleExceptionAsync(HttpContext context, int statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(new GenericResponse<string>()
            {
                ResponseCode = statusCode,
                Message = message
            }.ToString());
        }
    }
}
