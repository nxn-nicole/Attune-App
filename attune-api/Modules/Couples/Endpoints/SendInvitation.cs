using System.Security.Claims;
using attune_api.Data;
using attune_api.Infrastructure.SignalR;
using attune_api.Modules.Couples.Domain;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using static attune_api.Modules.Couples.Domain.CoupleInvitation;

namespace attune_api.Modules.Couples.Endpoints;

public static class SendInvitation
{
    public static async Task<IResult> Handle(
        Request request, ClaimsPrincipal principal, AttuneAppDbContext db,
        IHubContext<SignalRHub> hub)
    {
        var senderId = principal.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var receiverId = request.ReceiverId;

        if (receiverId == senderId)
            return Results.BadRequest("You can't invite yourself.");

        var users = await db.Users
            .Where(u => u.Id == senderId || u.Id == receiverId)
            .Select(u => new { u.Id, u.CoupleId })
            .ToListAsync();

        if (users.All(u => u.Id != receiverId))
            return Results.NotFound("User not found.");
        if (users.Any(u => u.CoupleId != null))
            return Results.Conflict("One of you is already paired.");

        // ponytail: check-then-insert, two simultaneous sends can both pass; add a unique index if it matters
        var alreadyPending = await db.CoupleInvitations.AnyAsync(i =>
            i.Status == InvitationStatus.Pending &&
            ((i.SenderId == senderId && i.ReceiverId == receiverId) ||
             (i.SenderId == receiverId && i.ReceiverId == senderId)));
        if (alreadyPending)
            return Results.Conflict("There's already a pending invitation between you.");

        var invitation = new CoupleInvitation
        {
            Id = Guid.NewGuid(),
            SenderId = senderId,
            ReceiverId = receiverId,
            Status = InvitationStatus.Pending,
            CreatedAt = DateTimeOffset.UtcNow
        };
        db.CoupleInvitations.Add(invitation);
        await db.SaveChangesAsync();

        // after save: never notify about an invitation that failed to persist
        await hub.Clients.User(receiverId).SendAsync("InvitationReceived", new
        {
            invitation.Id,
            SenderId = senderId,
            invitation.CreatedAt
        });

        return Results.Ok(new { invitation.Id });
    }
    
    public record Request(string ReceiverId);
}
