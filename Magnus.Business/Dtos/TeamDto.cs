namespace Magnus.Business.Dtos
{
	using System.Collections.Generic;
	using Domain;

	public class TeamDto
	{
		public virtual ICollection<UserDto> Users { get; set; }
		public Project Project { get; set; }
		public string Name { get; set; }
	}
}