using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CheckDrive.Api.Filters;

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            // Enum.GetNames returns the names of the enum values
            var enumNames = System.Enum.GetNames(context.Type);

            schema.Enum.Clear();
            foreach (var name in enumNames)
            {
                schema.Enum.Add(new OpenApiString(name));
            }
        }
    }
}
