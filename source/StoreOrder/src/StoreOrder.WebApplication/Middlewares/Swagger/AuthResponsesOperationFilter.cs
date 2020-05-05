using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Net;

namespace StoreOrder.WebApplication.Middlewares.Swagger
{
    public class AuthResponsesOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<AuthorizeAttribute>();

            if (authAttributes.Any())
            {
                operation.Responses.Add(((int)HttpStatusCode.BadRequest).ToString(), new OpenApiResponse { Description = "BadRequest" });
                operation.Responses.Add(((int)HttpStatusCode.Unauthorized).ToString(), new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add(((int)HttpStatusCode.NotFound).ToString(), new OpenApiResponse { Description = "NotFound" });
                operation.Responses.Add(((int)HttpStatusCode.Conflict).ToString(), new OpenApiResponse { Description = "Conflict" });
                operation.Responses.Add(((int)HttpStatusCode.InternalServerError).ToString(), new OpenApiResponse { Description = "InternalServerError" });
                operation.Responses.Add(((int)HttpStatusCode.BadGateway).ToString(), new OpenApiResponse { Description = "BadGateway" });
                operation.Responses.Add(((int)HttpStatusCode.NoContent).ToString(), new OpenApiResponse { Description = "NoContent" });
            }
        }
    }
}
