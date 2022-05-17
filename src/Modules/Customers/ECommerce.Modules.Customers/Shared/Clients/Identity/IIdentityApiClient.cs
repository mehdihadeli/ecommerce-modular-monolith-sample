using ECommerce.Modules.Customers.Shared.Clients.Identity.Dtos;

namespace ECommerce.Modules.Customers.Shared.Clients.Identity;

public interface IIdentityApiClient
{
    Task<GetUserByEmailResponse?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<CreateUserResponse?> CreateUserIdentityAsync(
        CreateUserRequest createUserRequest,
        CancellationToken cancellationToken = default);
}
