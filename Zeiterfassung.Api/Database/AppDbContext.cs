using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Zeiterfassung.Api.Database;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Tracking> Trackings { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        ChangeTracker.StateChanged += StateChanged;
        ChangeTracker.Tracked += StateChanged;
    }

    private void StateChanged(object? sender, EntityEntryEventArgs e)
    {
        if (e.Entry.Entity is User user && e.Entry.State == EntityState.Added)
            user.CreatedAt = DateTime.UtcNow;
    }
}
