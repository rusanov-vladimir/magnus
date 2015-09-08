namespace Client
{
	using Magnus.Business.Domain.Mappings;
	using Magnus.Business.Services.Interfaces;
	using Microsoft.Practices.Prism.Mvvm;
	using Ninject;
	using Ninject.Extensions.Conventions;

	public class AppBootstrapper
	{
		public static void RegisterDependencies()
		{
			IKernel kernel = new StandardKernel();

			 kernel.Bind(x => x
			.FromAssemblyContaining(typeof(AppBootstrapper), typeof(ILoginService))
			.SelectAllClasses()
			.BindDefaultInterface());

			kernel.Bind(x => x.FromAssemblyContaining(typeof(ILoginService))
							.SelectAllClasses()
							.InheritedFrom(typeof(IMap))
							.BindAllInterfaces());

			ViewModelLocationProvider.SetDefaultViewModelFactory(
				type => kernel.Get(type));
		}
	}
}