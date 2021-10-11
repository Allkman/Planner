using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Planner.API
{
    public class AssignContentTypeFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Responses.ContainsKey("200"))
            {
                operation.Responses.Clear();
            }

            var data = new OpenApiResponse
            {
                Description = "Ok",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType()
                }
            };

            operation.Responses.Add("200", data);
        }
    }
}
