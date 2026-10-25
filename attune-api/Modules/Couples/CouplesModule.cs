using attune_api.Modules.Couples.Endpoints;

namespace attune_api.Modules.Couples;

public static class CouplesModule
{
    public static IEndpointRouteBuilder MapCouplesModule(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/couples").RequireAuthorization();
        group.MapPost("/invitations", SendInvitation.Handle);
        return app;
    }
}
