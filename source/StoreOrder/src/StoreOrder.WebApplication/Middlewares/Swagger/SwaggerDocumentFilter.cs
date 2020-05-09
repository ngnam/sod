using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Middlewares.Swagger
{
    public class SwaggerDocumentFilter : IDocumentFilter
    {
        private readonly List<OpenApiTag> _tags = new List<OpenApiTag>
        {
            new OpenApiTag { Name = "Home", Description = "Browse the Home API" },
            new OpenApiTag { Name = "Account", Description = "Account API" },
            new OpenApiTag { Name = "Stores", Description = "Stores API"},
            new OpenApiTag { Name = "Location", Description = "Location API"},
            new OpenApiTag { Name = "UploadMedia", Description = "Upload Media API" }
        };
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            if (swaggerDoc == null)
            {
                throw new ArgumentNullException(nameof(swaggerDoc));
            }
            swaggerDoc.Tags = GetFilteredTagDefinitions(context);
            swaggerDoc.Paths = GetSortedPaths(swaggerDoc);
        }

        private List<OpenApiTag> GetFilteredTagDefinitions(DocumentFilterContext context)
        {
            //Filtering ensures route for tag is present
            var currentGroupNames = context.ApiDescriptions.Select(description => description.GroupName);
            return _tags.Where(tag => currentGroupNames.Contains(tag.Name)).ToList();
        }

        private OpenApiPaths GetSortedPaths(OpenApiDocument swaggerDoc)
        {
            return swaggerDoc.Paths;
        }
    }
}
