namespace Kitapix.Domain.Abstractions
{
	public abstract class Entity
	{

		public int Id { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime UpdatedDate { get; set; }


	}
}
