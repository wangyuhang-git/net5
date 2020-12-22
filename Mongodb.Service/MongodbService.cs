using Learn.Common;
using Learn.Models.Common;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mongodb.Service
{
    public class MongodbService<T> where T : BaseModel
    {
        protected readonly IMongoCollection<T> collection;

        public MongodbService(string tableName)
        {
            IMongoClient client = new MongoClient(ConfigHelper.GetSectionValue("ConnectionMongoDB:MongoDBAddress"));
            IMongoDatabase db = client.GetDatabase(ConfigHelper.GetSectionValue("ConnectionMongoDB:MongoDBName"));
            collection = db.GetCollection<T>(tableName);
        }

        public List<T> Get()
        {
            return collection.Find(t => true).ToList();
        }

        public T Get(string id)
        {
            return collection.Find<T>(t => t.Id == id).FirstOrDefault();
        }

        public T Add(T t)
        {
            collection.InsertOne(t);
            return t;
        }

        public void Update(string id, T tin)
        {
            collection.ReplaceOne(t => t.Id == id, tin);
        }

        public void Remove(string id)
        {
            collection.DeleteOne(t => t.Id == id);
        }

        public async Task<List<T>> GetAsync()
        {
            var filters = new List<FilterDefinition<T>>();

            filters.Add(Builders<T>.Filter.Empty);
            var filter = Builders<T>.Filter.And(filters);
            var fullCollection= await collection.FindAsync(filter);
            return await fullCollection.ToListAsync();
        }
    }
}
