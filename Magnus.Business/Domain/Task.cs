namespace Magnus.Business.Domain
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using Interfaces;

	public class Task : Entity, INamedEntity
	{
		public Task()
		{
			Documents = new List<Document>();
		}

		public virtual User CurrentUser { get; set; }
		public virtual Project Project { get; set; }
		public Guid? CurrentDocumentId { get; set; }
		public virtual ICollection<Document> Documents { get; set; }
		[NotMapped]
		public Document CurrentDocument
		{
			get
			{
				if (CurrentDocumentId == null)
					return null;
				return
					Documents.Single(x => x.Id == CurrentDocumentId);
			}
		}
		public string Name { get; set; }
	}
}