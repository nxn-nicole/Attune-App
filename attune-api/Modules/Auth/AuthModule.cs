using attune_api.Data;
using attune_api.Modules.Auth.Domain;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;

namespace attune_api.Modules.Auth;

public static class AuthModule
{
    public static IServiceCollection AddAuthModule(this IServiceCollection services)
    {
        services.AddIdentityApiEndpoints<AppUser>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<AttuneAppDbContext>();

        services.Configure<BearerTokenOptions>(IdentityConstants.BearerScheme, options =>
        {
            options.Events.OnMessageReceived = context =>
            {
                var token = context.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(token) && context.HttpContext.Request.Path.StartsWithSegments("/hub"))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            };
        });
        services.AddAuthorization();
        return services;
    }
}