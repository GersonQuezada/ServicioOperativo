using Credimujer.Op.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Api.Filter
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var listaError = new List<string>();
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.Where(v => v.Errors.Count > 0)
                     .SelectMany(v => v.Errors)
                     .Select(v => v.ErrorMessage)
                     .ToList();

                var errorResponse = new Response
                {
                    Status = Constants.SystemStatusCode.Required,
                    Message = "Campos requeridos",
                    Data = errors
                };

                context.Result = new OkObjectResult(errorResponse);
                return;
            }

            await next();
        }
    }

    public class Response
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }
}