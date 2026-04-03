namespace Library.Infrastructure.Persistence.Interceptors
{
	public class AuditableEntityInterceptor(IUserContext userContext) : SaveChangesInterceptor
	{
		public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
		{
			UpdateAuditableEntities(eventData.Context);

			return base.SavingChanges(eventData, result);
		}

		public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
		{
			UpdateAuditableEntities(eventData.Context);

			return base.SavingChangesAsync(eventData, result, cancellationToken);
		}

		private void UpdateAuditableEntities(DbContext? context)
		{
			if (context == null) return;

			var userId = userContext.GetUserId();
			var utcNow = DateTime.UtcNow;

			foreach (var entry in context.ChangeTracker.Entries<IAuditable>())
			{
				if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
				{
					if (entry.State == EntityState.Added)
					{
						entry.Entity.CreatedByUserId = userId;
						entry.Entity.CreatedDate = utcNow;
					}

					entry.Entity.LastModifiedByUserId = userId;
					entry.Entity.LastModifiedDate = utcNow;
				}
			}
		}
	}
}
