using EAnalytics.Common.Events;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EAnalytics.Common.Primitives.Models
{
    public class EventModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public Guid AggregateIdentifier { get; set; }
        public string AggregateType { get; set; }
        public int Version { get; set; }
        public string EventType { get; set; }
        public BaseEvent EventData { get; set; }
    }
}