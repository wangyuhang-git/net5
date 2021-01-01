using Learn.Common;
using Learn.Models.Common;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mongodb.Service
{
    public class MongodbService<T, S>
        where T : BaseModel
        where S : BaseSearchModel

    {
        protected readonly IMongoCollection<T> collection;

        public MongodbService(string tableName)
        {
            IMongoClient client = new MongoClient(ConfigHelper.GetSectionValue("ConnectionMongoDB:MongoDBAddress"));
            IMongoDatabase db = client.GetDatabase(ConfigHelper.GetSectionValue("ConnectionMongoDB:MongoDBName"));
            collection = db.GetCollection<T>(tableName);
        }


        public T Get(string id)
        {
            return collection.Find<T>(t => t.Id == id).FirstOrDefault();
        }

        public async Task<T> GetAsync(string id)
        {
            IAsyncCursor<T> asyncCursor = await collection.FindAsync(t => t.Id == id);
            List<T> list = await asyncCursor.ToListAsync();
            return list.FirstOrDefault();
        }

        public T Add(T t)
        {
            collection.InsertOne(t);
            return t;
        }

        public async Task<T> AddAsync(T t)
        {
            await collection.InsertOneAsync(t);
            return t;
        }

        public void AddMany(IEnumerable<T> list)
        {
            collection.InsertMany(list);
        }

        public async Task AddManyAsync(IEnumerable<T> list)
        {
            await collection.InsertManyAsync(list);
        }

        public void Update(string id, T tin)
        {
            collection.ReplaceOne(t => t.Id == id, tin);
        }

        public async Task UpdataAsync(string id, T tin)
        {
            await collection.ReplaceOneAsync(t => t.Id == id, tin);
        }

        public void Remove(string id)
        {
            collection.DeleteOne(t => t.Id == id);
        }

        public async Task RemoveAsync(string id)
        {
            await collection.DeleteOneAsync(t => t.Id == id);
        }

        public List<T> GetAllList()
        {
            var filters = new List<FilterDefinition<T>>();
            filters.Add(Builders<T>.Filter.Empty);
            var filter = Builders<T>.Filter.And(filters);
            var fullCollection = collection.Find(filter);
            return fullCollection.ToList();
        }

        public async Task<List<T>> GetAllListAsync()
        {
            var filters = new List<FilterDefinition<T>>();

            filters.Add(Builders<T>.Filter.Empty);
            var filter = Builders<T>.Filter.And(filters);
            var fullCollection = await collection.FindAsync(filter);
            return await fullCollection.ToListAsync();
        }

        public List<T> GetList(Expression<Func<T, bool>> expression)
        {
            return collection.Find(expression).ToList();
        }

        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> expression)
        {
            var filters = new List<FilterDefinition<T>>();
            filters.Add(GetAction(expression));
            var filter = Builders<T>.Filter.And(filters);
            var fullCollectioin = await collection.FindAsync(filter);
            return await fullCollectioin.ToListAsync();
        }

        public FilterDefinition<T> GetAction(Expression<Func<T, bool>> expression)
        {
            if (null == expression)
            {
                return Builders<T>.Filter.Empty;
            }
            else
            {
                return Builders<T>.Filter.Where(expression);
            }
        }
    }
}
