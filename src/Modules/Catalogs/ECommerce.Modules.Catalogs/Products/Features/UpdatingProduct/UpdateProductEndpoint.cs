using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.Web;

namespace ECommerce.Modules.Catalogs.Products.Features.UpdatingProduct;

// PUT api/v1/catalog/products/{id}
public static class UpdateProductEndpoint
{
    internal static IEndpointRouteBuilder MapCreateProductsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                $"{ProductsConfigs.ProductsPrefixUri}/{{id}}",
                UpdateProducts)
            .WithTags(ProductsConfigs.Tag)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("UpdateProduct")
            .WithDisplayName("Update a product.");

        return endpoints;
    }

    private static Task<IResult> UpdateProducts(
        long id,
        UpdateProductRequest request,
        IGatewayProcessor<CatalogModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        return gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            var command = new UpdateProduct(
                id,
                request.Name,
                request.Price,
                request.RestockThreshold,
                request.MaxStockThreshold,
                request.Status,
                request.Width,
                request.Height,
                request.Depth,
                request.Size,
                request.CategoryId,
                request.SupplierId,
                request.BrandId,
                request.Description);

            await commandProcessor.SendAsync(command, cancellationToken);
            return Results.NoContent();
        });
    }
}
