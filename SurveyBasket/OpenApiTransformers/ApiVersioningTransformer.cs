using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace SurveyBasket.OpenApiTransformers;

public sealed class ApiVersioningTransformer(ApiVersionDescription description) : IOpenApiDocumentTransformer
{
    private readonly ApiVersionDescription _description = description;
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {


        document.Info = new()
        {

            Title = "Survey Basket API",
            Version = _description.ApiVersion.ToString(),
            Description = $"API Description.{(_description.IsDeprecated ? " This API version has been deprecated." : string.Empty)}",
        };
        return Task.CompletedTask;
    }
}
