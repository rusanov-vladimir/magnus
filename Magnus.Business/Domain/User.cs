namespace Magnus.Business.Domain
{
	using Interfaces;

	public class User : Entity
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public bool IsAdvancedUser { get; set; }
		public Team Team { get; set; }
	}
}