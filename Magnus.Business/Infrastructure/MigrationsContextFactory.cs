namespace Magnus.Business.Infrastructure
{
	using System.Data.Entity.Infrastructure;
	using System.Linq;
	using Domain.Mappings;
	using Ninject;
	using Ninject.Extensions.Conventions;

	public class MigrationsContextFactory : IDbContextFactory<Repository>
	{
		public Repository Create()
		{
			var kernel = new StandardKernel();
			kernel.Bind(x => x.FromAssemblyContaining(typeof(IMap))
							.SelectAllClasses()
							.InheritedFrom(typeof(IMap))
							.BindAllInterfaces());
			var maps = kernel.GetAll<IMap>().ToArray();
			return new Repository(maps);
		}
	}
}