using attune_api.Modules.Auth.Domain;
using attune_api.Modules.Couples.Domain;
using attune_api.Modules.Messages.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace attune_api.Data;

public class AttuneAppDbContext(DbContextOptions<AttuneAppDbContext> options) : IdentityDbContext<AppUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<AppUser>()
            .HasOne(u => u.CoupleUser)
            .WithOne()
            .HasForeignKey<AppUser>(u => u.CoupleId)
            .OnDelete(DeleteBehavior.SetNull);
    }
    // Auth:
    // Users (DbSet<AppUser>) comes from IdentityDbContext<AppUser>

    // Message
    public DbSet<Message> Messages => Set<Message>();

    // Couple
    public DbSet<CoupleInvitation> CoupleInvitations => Set<CoupleInvitation>();
}
