namespace Magnus.Business.Migrations
{
	using System.Data.Entity.Migrations;
	using Infrastructure;

	internal sealed class Configuration : DbMigrationsConfiguration<Repository>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
		}
	}
}
