using attune_api.Data;
using attune_api.Infrastructure.SignalR;
using attune_api.Modules.Auth;
using attune_api.Modules.Auth.Domain;
using attune_api.Modules.Couples;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var cs = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Connection string 'Default' not found.");

builder.Services.AddDbContext<AttuneAppDbContext>(o => o.UseNpgsql(cs));
builder.Services
    .AddAuthModule();
builder.Services.AddOpenApi();
builder.Services.AddSignalR();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapHub<SignalRHub>("/hub");
app.MapGroup("/auth").MapIdentityApi<AppUser>();
app.MapCouplesModule();

app.Run();