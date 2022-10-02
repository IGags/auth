using System;
using System.Threading.Tasks;
using Api.Areas.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters
{
    public class ApiExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {

            var model = new ExceptionResponse()
            {
                StatusCode = context.HttpContext.Response.StatusCode.ToString(),
                Message = context.Exception.Message
            };

            context.Result = new JsonResult(model);
        }
    }
}
