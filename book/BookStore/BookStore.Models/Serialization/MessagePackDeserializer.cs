using MessagePack;
using Confluent.Kafka;

namespace BookStore.Models.Serialization
{
    public class MessagePackDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return MessagePackSerializer.Deserialize<T>(data.ToArray());
        }
    }
}
