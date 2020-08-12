using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PakingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PakingAPI
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthentication : Attribute, IAsyncActionFilter
    {
        private const string Username = "APIUsername";
        private const string Password = "APIPassword";
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

            using(var dataContext= new DataContext())
            {
                var credentials = dataContext.UserAccounts.Where(x => x.Username == potentialUsername.ToString() &&
                 x.Password == potentialPassword.ToString()).FirstOrDefault();
                if (credentials==null)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
                await next();
            }
          
        }
    }
}
