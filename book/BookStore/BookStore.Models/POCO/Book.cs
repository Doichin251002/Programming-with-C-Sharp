using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MessagePack;

namespace BookStore.Models.POCO
{
    [MessagePackObject]
    public class Book : ICacheItem<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [Key(0)]
        public string Id { get; set; }

        [Key(1)]
        public string Title { get; set; }

        [Key(4)]
        public int Year { get; set; }

        [Key(5)]
        public List<string> Authors { get; set; }

        [Key(6)]
        public DateTime DateInserted { get; set; }

        public string GetKey()
        {
            return Id;
        }
    }
}
