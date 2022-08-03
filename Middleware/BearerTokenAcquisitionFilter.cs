using Microsoft.AspNetCore.Mvc.Filters;

namespace clinic_assessment_redone.Middleware
{
    public class BearerTokenAcquisitionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var assertion = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            context.HttpContext.Request.Headers.Add("BearerToken", assertion);
            await next.Invoke();
        }
    }
}
