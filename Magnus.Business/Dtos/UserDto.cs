namespace Magnus.Business.Dtos
{
	using System;

	public class UserDto
	{
		public Guid Id { get; set; } 
		public string Username { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }
		public int AuthenticationType { get; set; }
		public bool IsAdvancedUser { get; set; }
		public ProjectDto Project { get; set; }
	}
}