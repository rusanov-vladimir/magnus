namespace Magnus.Business.Domain
{
	using System.Collections.Generic;
	using Interfaces;

	public class Team :Entity, INamedEntity
	{
		public virtual ICollection<User> Users { get; set; }
		public Project Project { get; set; }
		public string Name { get; set; }

		public Team()
		{
			Users = new List<User>();
		}
	}
}