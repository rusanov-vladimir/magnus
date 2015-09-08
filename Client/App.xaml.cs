using System.Windows;

namespace Client
{
	using System;
	using System.Security.Principal;
	using Magnus.Business.Infrastructure;

	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		#region Overrides of Application

		protected override void OnStartup(StartupEventArgs e)
		{
			AppBootstrapper.RegisterDependencies();
			AppDomain.CurrentDomain.SetThreadPrincipal(new AppPrincipal());

		}

		#endregion
	}
}
