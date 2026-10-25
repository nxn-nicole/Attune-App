using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using attune_api.Modules.Auth.Domain;

namespace attune_api.Modules.Messages.Domain;

public class Message
{
    public required Guid Id { get; set; }
    public required string Content { get; set; }
    public required string SenderId { get; set; }
    public required string ReceiverId { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }

    #region Navigation
    [ForeignKey(nameof(SenderId))]
    public AppUser Sender { get; set; }
    [ForeignKey(nameof(ReceiverId))]
    public AppUser Receiver { get; set; }
    #endregion
    
}