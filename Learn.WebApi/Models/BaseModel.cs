﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn.WebApi.Models
{
    public class BaseModel
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }


        [BsonElement(nameof(CreateTime))]
        public DateTime CreateTime { get; set; }

        public bool IsDelete { get; set; }

        public BaseModel()
        {
            this.CreateTime = DateTime.Now;
            this.IsDelete = false;
        }
    }
}
