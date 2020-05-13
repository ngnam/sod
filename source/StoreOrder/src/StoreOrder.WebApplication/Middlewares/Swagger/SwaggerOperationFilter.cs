using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Middlewares.Swagger
{
    public class SwaggerOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            ValidateInput(operation, context);
            if (IsMethodWithHttpGetAttribute(context))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "extra",
                    In = ParameterLocation.Query,
                    Description = "This is an extra querystring parameter",
                    Required = false,
                });

                //operation.Parameters.Add(new OpenApiParameter
                //{
                //    Name = "Authorization",
                //    In = ParameterLocation.Header,
                //    Description = "Used for certain authorization policies such as Bearer token authentication",
                //    Required = false,
                //});
            }
        }

        private void ValidateInput(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }
        }

        private bool IsMethodWithHttpGetAttribute(OperationFilterContext context)
        {
            return context.MethodInfo.CustomAttributes.Any(attribute => attribute.AttributeType == typeof(HttpGetAttribute));
        }
    }
}
