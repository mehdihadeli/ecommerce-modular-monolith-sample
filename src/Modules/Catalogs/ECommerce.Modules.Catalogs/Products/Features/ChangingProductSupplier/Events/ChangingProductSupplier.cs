using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Event.Internal;
using BuildingBlocks.Core.CQRS.Event.Internal;
using ECommerce.Modules.Catalogs.Shared.Contracts;
using ECommerce.Modules.Catalogs.Shared.Extensions;
using ECommerce.Modules.Catalogs.Suppliers;

namespace ECommerce.Modules.Catalogs.Products.Features.ChangingProductSupplier.Events;

public record ChangingProductSupplier(SupplierId SupplierId) : DomainEvent;

internal class ChangingSupplierValidationHandler :
    IDomainEventHandler<ChangingProductSupplier>
{
    private readonly ICatalogDbContext _catalogDbContext;

    public ChangingSupplierValidationHandler(ICatalogDbContext catalogDbContext)
    {
        _catalogDbContext = catalogDbContext;
    }

    public async Task Handle(ChangingProductSupplier notification, CancellationToken cancellationToken)
    {
        Guard.Against.Null(notification, nameof(notification));
        Guard.Against.NegativeOrZero(notification.SupplierId, nameof(notification.SupplierId));
        Guard.Against.ExistsSupplier(
            await _catalogDbContext.SupplierExistsAsync(notification.SupplierId, cancellationToken: cancellationToken),
            notification.SupplierId);
    }
}
