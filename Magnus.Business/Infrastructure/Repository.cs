namespace Magnus.Business.Infrastructure
{
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Core.Objects;
	using System.Linq;
	using System.Linq.Expressions;
	using Domain.Interfaces;
	using Domain.Mappings;

	public class Repository : DatabaseContext, IRepository
	{
		public Repository(IMap[] dbMappings) : base(dbMappings)
		{

		}

		public IQueryable<TEntity> Query<TEntity>() where TEntity : class
		{
			return Set<TEntity>();
		}

		public IQueryable Query(Type entityType)
		{
			return Set(entityType);
		}

		public TEntity Add<TEntity>(TEntity entity) where TEntity : class
		{
			Set<TEntity>().Add(entity);
			return entity;
		}

		public void Delete<TEntity>(TEntity entity) where TEntity : class
		{
			Set<TEntity>().Remove(entity);
		}

		public void Delete(object entity)
		{
			ObjectContext.DeleteObject(entity);
		}

		public void DeleteNavProperty<TEntity>(TEntity parentObject, Expression<Func<TEntity, object>> detachProperty)
			where TEntity : class
		{
			object value = detachProperty.Compile()(parentObject);
			if (value == null)
				return;
			ObjectContext.ObjectStateManager.ChangeRelationshipState(parentObject, value, detachProperty, EntityState.Deleted);
		}

		public TEntity CreateAndAttach<TEntity>(Guid id) where TEntity : class, IEntity
		{
			if (id == Guid.Empty)
				return null;
			var entity = Set<TEntity>().Create<TEntity>();
			Entry(entity).Property(x => x.Id).CurrentValue = id;
			if (Set<TEntity>().Local.Any(x => x.Id == id))
				return Set<TEntity>().Local.FirstOrDefault(x => x.Id == id);
			Set<TEntity>().Attach(entity);
			return entity;
		}

		public void Commit()
		{
			base.SaveChanges();
			RefreshAll();

		}

		public void Refresh(IEntity entity)
		{
			ObjectContext.Refresh(RefreshMode.StoreWins, entity);
		}

		public void RefreshAll()
		{
			var refreshableObjects = (from entry in ObjectContext.ObjectStateManager.GetObjectStateEntries(
											   EntityState.Deleted
											 | EntityState.Modified
											 | EntityState.Unchanged)
									  where entry.EntityKey != null && entry.Entity != null
									  select entry.Entity);

			ObjectContext.Refresh(RefreshMode.StoreWins, refreshableObjects);
		}
	}
}