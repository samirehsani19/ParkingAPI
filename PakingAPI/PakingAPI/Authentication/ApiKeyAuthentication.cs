using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PakingAPI
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthentication : Attribute, IAsyncActionFilter
    {
        private const string Username = "API Username";
        private const string Password = "API Password";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(Username, out var potentialUsername))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!context.HttpContext.Request.Headers.TryGetValue(Password, out var potentialPassword))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}
