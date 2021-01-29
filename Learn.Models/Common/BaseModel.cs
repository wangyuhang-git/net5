using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Models.Common
{
    [BsonIgnoreExtraElements]
    public class BaseModel
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [BsonElement(nameof(Id))]
        public string Id { get; set; }

        [BsonElement(nameof(CreateTime))]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateTime { get; set; }

        [BsonElement(nameof(IsDelete))]
        public bool IsDelete { get; set; }

        public BaseModel()
        {
            this.CreateTime = DateTime.Now;
            this.IsDelete = false;
        }
    }
}
