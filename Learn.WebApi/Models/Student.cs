using System;

namespace Learn.WebApi.Models
{
    //[MongoDB.Bson.Serialization.Attributes.BsonIgnoreExtraElements]
    public class Student : BaseModel
    {
        //public object _id { get; set; }

        public string StudentId
        {
            get; set;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Birthday { get; set; }
    }
}
