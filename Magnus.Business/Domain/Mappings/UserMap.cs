namespace Magnus.Business.Domain.Mappings
{
	public class UserMap : Map<User>
	{
		public UserMap()
		{
			Property(x => x.Username).HasMaxLength(64).IsRequired();
			Property(x => x.FirstName).HasMaxLength(64);
			Property(x => x.LastName).HasMaxLength(64);
		}
	}
}