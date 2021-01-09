using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Models.Common
{
    public class BaseModel
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement(nameof(CreateTime))]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateTime { get; set; }

        public bool IsDelete { get; set; }

        public BaseModel()
        {
            this.CreateTime = DateTime.Now;
            this.IsDelete = false;
        }
    }
}
