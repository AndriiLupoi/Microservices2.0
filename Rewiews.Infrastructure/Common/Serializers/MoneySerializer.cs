using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Rewiews.Domain.ValueObjects;

namespace Rewiews.Infrastructure.Common.Serializers
{
    public class MoneySerializer : SerializerBase<Money>
    {
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Money value)
        {
            context.Writer.WriteStartDocument();
            context.Writer.WriteString(nameof(Money.Currency), value.Currency);
            context.Writer.WriteDecimal128(nameof(Money.Amount), value.Amount);
            context.Writer.WriteEndDocument();
        }

        public override Money Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            context.Reader.ReadStartDocument();
            var currency = context.Reader.ReadString(nameof(Money.Currency));
            var amount = context.Reader.ReadDecimal128(nameof(Money.Amount));
            context.Reader.ReadEndDocument();
            
            return new Money(Decimal128.ToDecimal(amount), currency);
        }
    }
}
