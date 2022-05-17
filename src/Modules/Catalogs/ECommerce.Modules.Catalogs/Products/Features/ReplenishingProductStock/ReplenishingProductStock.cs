using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Core.Exception;
using ECommerce.Modules.Catalogs.Products.Exceptions.Application;
using ECommerce.Modules.Catalogs.Shared.Contracts;
using ECommerce.Modules.Catalogs.Shared.Extensions;

namespace ECommerce.Modules.Catalogs.Products.Features.ReplenishingProductStock;

public record ReplenishingProductStock(long ProductId, int Quantity) : ITxCommand;

internal class ReplenishingProductStockValidator : AbstractValidator<ReplenishingProductStock>
{
    public ReplenishingProductStockValidator()
    {
        // https://docs.fluentvalidation.net/en/latest/conditions.html#stop-vs-stoponfirstfailure
        CascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Quantity)
            .GreaterThan(0);

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("ProductId must be greater than 0");
    }
}

internal class ReplenishingProductStockHandler : ICommandHandler<ReplenishingProductStock>
{
    private readonly ICatalogDbContext _catalogDbContext;

    public ReplenishingProductStockHandler(ICatalogDbContext catalogDbContext)
    {
        _catalogDbContext = Guard.Against.Null(catalogDbContext, nameof(catalogDbContext));
    }

    public async Task<Unit> Handle(ReplenishingProductStock command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(command));

        var product = await _catalogDbContext.FindProductByIdAsync(command.ProductId);
        Guard.Against.NotFound(product, new ProductNotFoundException(command.ProductId));

        product!.ReplenishStock(command.Quantity);
        await _catalogDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
