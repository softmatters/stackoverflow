using System;
using System.Globalization;
using AuthHeader.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace AuthHeader.Attributes
{
    public class ValidateAuthHeaderAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;

            if (httpContext.Request.Headers.ContainsKey("Authorization"))
            {
                return;
            }

            var failureResponse = new FailiureResponseModel
            {
                Result = false,
                ResultDetails = "Authorization header not present in request",
                Uri = httpContext.Request.Path.ToUriComponent(),
                Timestamp = DateTime.Now.ToString("s", CultureInfo.InvariantCulture),
                Error = new Error
                {
                    Code = 108,
                    Description = "Authorization header not present in request",
                    Resolve = "Send Request with authorization header to avoid this error."
                }
            };

            var responseString = JsonConvert.SerializeObject(failureResponse);

            context.Result = new ContentResult
            {
                Content = responseString,
                ContentType = "application/json",
                StatusCode = 403
            };
        }
    }
}