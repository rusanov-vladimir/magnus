namespace Magnus.Business.Infrastructure
{
	using System;
	using System.Security.Principal;

	public class AppIdentity : IIdentity
	{
		public AppIdentity(Guid userId, string name, Guid projectId, string email, string[] roles)
		{
			UserId = userId;
			Name = name;
			ProjectId = projectId;
			Email = email;
			Roles = roles;
		}

		public Guid UserId { get; set; }
		public string Name { get; private set; }
		public Guid ProjectId { get; set; }
		public string Email { get; private set; }
		public string[] Roles { get; private set; }

		#region IIdentity Members
		public string AuthenticationType { get { return "Custom authentication"; } }

		public bool IsAuthenticated { get { return !string.IsNullOrEmpty(Name); } }
		#endregion
	}
}