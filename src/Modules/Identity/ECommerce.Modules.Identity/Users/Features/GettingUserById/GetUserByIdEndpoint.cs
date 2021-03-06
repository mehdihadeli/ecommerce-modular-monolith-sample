using BuildingBlocks.Abstractions.Web;
using ECommerce.Modules.Identity.Users.Features.RegisteringUser;

namespace ECommerce.Modules.Identity.Users.Features.GettingUserById;

public static class GetUserByIdEndpoint
{
    internal static IEndpointRouteBuilder MapGetUserByIdEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet($"{UsersConfigs.UsersPrefixUri}/{{userId:guid}}", GetUserById)
            .AllowAnonymous()
            .WithTags(UsersConfigs.Tag)
            .Produces<RegisterUserResult>(StatusCodes.Status200OK)
            .Produces<RegisterUserResult>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("GetUserById")
            .WithDisplayName("Get User by Id.");

        return endpoints;
    }

    private static Task<IResult> GetUserById(
        Guid userId,
        IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        return gatewayProcessor.ExecuteQuery(async queryProcessor =>
        {
            var result = await queryProcessor.SendAsync(new GetUserById(userId), cancellationToken);

            return Results.Ok(result);
        });
    }
}
