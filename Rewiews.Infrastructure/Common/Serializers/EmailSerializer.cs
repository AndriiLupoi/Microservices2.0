using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Rewiews.Domain.ValueObjects;

namespace Rewiews.Infrastructure.Common.Serializers
{
    public class EmailSerializer : SerializerBase<Email>
    {
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Email value)
        {
            context.Writer.WriteStartDocument();
            context.Writer.WriteString(nameof(Email.Value), value.Value);
            context.Writer.WriteEndDocument();
        }

        public override Email Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            context.Reader.ReadStartDocument();
            var value = context.Reader.ReadString(nameof(Email.Value));
            context.Reader.ReadEndDocument();

            return new Email(value);
        }
    }
}
