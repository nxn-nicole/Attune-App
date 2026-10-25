using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace attune_api.Infrastructure.SignalR;

[Authorize]
public class SignalRHub :Hub
{
    
}