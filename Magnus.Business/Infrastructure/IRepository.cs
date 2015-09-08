namespace Magnus.Business.Infrastructure
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using Domain;
	using Domain.Interfaces;

	public interface IRepository: IDisposable
	{
		IQueryable<TEntity> Query<TEntity>() where TEntity : class;
		IQueryable Query(Type entityType);
		TEntity Add<TEntity>(TEntity entity) where TEntity : class;
		void Delete<TEntity>(TEntity entity) where TEntity : class;
		void Delete(object entity);

		void DeleteNavProperty<TEntity>(TEntity parentObject, Expression<Func<TEntity, object>> detachProperty)
			where TEntity : class;

		TEntity CreateAndAttach<TEntity>(Guid id) where TEntity : class, IEntity;
		void Commit();
		void Refresh(IEntity entity);
	}
}