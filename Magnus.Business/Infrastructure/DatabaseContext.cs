namespace Magnus.Business.Infrastructure
{
	using System.Data.Entity;
	using System.Data.Entity.Core.Objects;
	using System.Data.Entity.Infrastructure;
	using Domain;
	using Domain.DynamicFields;
	using Domain.Mappings;

	public class DatabaseContext : DbContext
	{
		private readonly IMap[] _dbMappings;
		public IDbSet<User> Users { get; set; }
		public IDbSet<DynamicFieldTemplate> DynamicFieldTemplates { get; set; }
		public IDbSet<Project> Projects { get; set; }
		public IDbSet<Task> Tasks { get; set; } 
		public IDbSet<Team> Teams { get; set; } 

		public ObjectContext ObjectContext
		{
			get { return ((IObjectContextAdapter)this).ObjectContext; }
		}

		static DatabaseContext()
		{
			Database.SetInitializer(new DatabaseInitializer());
		}

		public DatabaseContext(IMap[] dbMappings)
			: base("DatabaseContext")
		{
			_dbMappings = dbMappings;
			Database.Initialize(false);
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			foreach (dynamic map in _dbMappings)
			{
				modelBuilder.Configurations.Add(map);
			}
		}
	}
}