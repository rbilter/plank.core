using Plank.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Plank.Core.Data
{
    public abstract class PlankDbContext(DbContextOptions options) : DbContext(options)
    {
        public void DetachAllEntities()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.State != EntityState.Detached)
                .ToList();

            foreach (var entry in entries)
            {
                if (entry.Entity != null)
                {
                    entry.State = EntityState.Detached;
                }
            }
        }

        public Task<int> SaveChanges(CancellationToken cancellationToken = default(CancellationToken))
        {
            ChangeTracker.Entries()
                .Where(a => (a.State == EntityState.Added || a.State == EntityState.Modified))
                .ToList()
                .ForEach(a =>
                {
                    if (a.Entity is IPopulateComputedColumns timeStamps)
                    {
                        timeStamps.PopulateComputedColumns();
                    }
                });

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}