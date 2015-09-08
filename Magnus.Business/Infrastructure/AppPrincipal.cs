namespace Magnus.Business.Infrastructure
{
	using System.Linq;
	using System.Security.Principal;

	public class AppPrincipal : IPrincipal
	{
		private AppIdentity _identity;

		public AppIdentity Identity
		{
			get { return _identity; }
			set { _identity = value; }
		}

		IIdentity IPrincipal.Identity
		{
			get { return this.Identity; }
		}

		public bool IsInRole(string role)
		{
			return _identity.Roles.ToList().Contains(role);
		}
	}
}