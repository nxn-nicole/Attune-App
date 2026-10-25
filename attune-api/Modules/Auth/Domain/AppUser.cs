using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace attune_api.Modules.Auth.Domain;

public class AppUser : IdentityUser
{
    public string? CoupleId { get; set; }

    #region Navigation

    [ForeignKey(nameof(CoupleId))]
    public AppUser? CoupleUser { get; set; }

    #endregion
}