using System.ComponentModel.DataAnnotations.Schema;
using attune_api.Modules.Auth.Domain;

namespace attune_api.Modules.Couples.Domain;

public class CoupleInvitation
{
    public required Guid Id { get; set; }
    public required string SenderId { get; set; }
    public required string ReceiverId { get; set; }
    public required InvitationStatus Status { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }

    #region Navigation
    [ForeignKey(nameof(SenderId))]
    public AppUser Sender { get; set; }
    [ForeignKey(nameof(ReceiverId))]
    public AppUser Receiver { get; set; }
    #endregion
    
    public enum InvitationStatus
    {
        Pending,
        Declined,
        Accepted
    }
}