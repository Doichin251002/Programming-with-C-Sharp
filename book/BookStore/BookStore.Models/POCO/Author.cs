using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MessagePack;

namespace BookStore.Models.POCO
{
    [MessagePackObject]
    public class Author : ICacheItem<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [Key(0)]
        public string Id { get; set; }

        [Key(1)]
        public string Name { get; set; }

        [Key(3)]
        public DateTime DateInserted { get; set; }

        public string GetKey()
        {
            return Id;
        }
    }
}
