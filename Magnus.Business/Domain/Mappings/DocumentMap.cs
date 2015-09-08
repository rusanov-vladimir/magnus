
namespace Magnus.Business.Domain.Mappings
{
	public class DocumentMap : Map<Document>
	{
		public DocumentMap()
		{
			HasMany(x => x.Fields).WithMany();
			Property(x => x.Name).HasMaxLength(256);
		}
	}
}
