﻿using Domain.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Persistence.Interceptors
{
    public class AuditableEntityInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            if (eventData.Context is not null)
            {
                AuditableEntities(eventData.Context);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private static void AuditableEntities(DbContext context)
        {
            DateTime utcNow = DateTime.UtcNow;
            var entities = context.ChangeTracker
                .Entries<IAuditableEntity>()
                .Where(c => c.State != EntityState.Unchanged)
                .ToList();

            foreach (EntityEntry<IAuditableEntity> entry in entities)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(nameof(IAuditableEntity.CreatedBy)).CurrentValue = 1;
                    entry.Property(nameof(IAuditableEntity.CreatedDateOnUtc)).CurrentValue = utcNow;
                    continue;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(IAuditableEntity.UpdatedBy)).CurrentValue = 1;
                    entry.Property(nameof(IAuditableEntity.UpdatedDateOnUtc)).CurrentValue = utcNow;
                }
            }
        }
    }    
}
