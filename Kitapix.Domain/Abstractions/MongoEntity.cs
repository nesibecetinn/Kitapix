using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Kitapix.Domain.Abstractions
{
	public abstract class MongoEntity
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
		public DateTime? CreatedDate { get; set; }

		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
		public DateTime? UpdatedDate { get; set; }
	}
}
