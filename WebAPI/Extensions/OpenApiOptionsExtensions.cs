using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace EdgIQ.Inventory.WebAPI.Extensions
{
    public static class OpenApiOptionsExtensions
    {
        public static OpenApiOptions AddBearerTokenAuthentication(this OpenApiOptions options)
        {
            //var scheme = new OpenApiSecurityScheme()
            //{
            //    Type = SecuritySchemeType.Http,
            //    Name = Microsoft.AspNetCore.Identity.IdentityConstants.BearerScheme,
            //    Scheme = "Bearer",
            //    Reference = new()
            //    {
            //        Type = ReferenceType.SecurityScheme,
            //        Id = Microsoft.AspNetCore.Identity.IdentityConstants.BearerScheme
            //    }
            //};
            //options.AddDocumentTransformer((document, context, cancellationToken) =>
            //{
            //    document.Components ??= new();
            //    document.Components.SecuritySchemes.Add(Microsoft.AspNetCore.Identity.IdentityConstants.BearerScheme, scheme);
            //    return Task.CompletedTask;
            //});
            //options.AddOperationTransformer((operation, context, cancellationToken) =>
            //{
            //    if (context.Description.ActionDescriptor.EndpointMetadata.OfType<IAuthorizeData>().Any())
            //    {
            //        operation.Security = [new() { [scheme] = [] }];
            //    }
            //    return Task.CompletedTask;
            //});
            return options;
        }
    }
}