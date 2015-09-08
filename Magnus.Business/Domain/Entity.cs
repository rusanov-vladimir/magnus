namespace Magnus.Business.Domain
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using Interfaces;

	public abstract class Entity : IEntity
	{
		private Guid _id;

		[Key]
		public Guid Id
		{
			get { return _id; }
			set { _id = value; }
		}

		protected Entity()
		{
			_id = Guid.NewGuid();
		}

	}
}