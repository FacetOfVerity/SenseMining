using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SenseMining.FPG.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var request = context.HttpContext.Request;
        if (request.Headers != null)
        {
            context.HttpContext.Response.StatusCode = 500;
            context.Result = new JsonResult(new { context.Exception.Message });
        }
    }
}