using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace PakingAPI
{
    internal class HeaderParameter:IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();
            var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

            if (descriptor != null && context.ApiDescription.HttpMethod.Equals("Post") ||
                context.ApiDescription.HttpMethod.Equals("PUT")|| context.ApiDescription.HttpMethod.Equals("Delete"))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "API Username",
                    In = ParameterLocation.Header,
                    Required = true
                }) ;

                operation.Parameters.Add(new OpenApiParameter
                { 
                    Name="API Password",
                    In =ParameterLocation.Header,
                    Required=true
                });
            }
            
        }
    }
}