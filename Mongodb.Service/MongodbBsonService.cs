using Learn.Common;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mongodb.Service
{
    /// <summary>
    /// BsonDocument文档操作（主要是管道）
    /// </summary>
    public class MongodbBsonService
    {
        protected readonly IMongoCollection<BsonDocument> collection;

        public MongodbBsonService(string tableName)
        {
            IMongoClient client = new MongoClient(ConfigHelper.GetSectionValue("ConnectionMongoDB:MongoDBAddress"));
            IMongoDatabase db = client.GetDatabase(ConfigHelper.GetSectionValue("ConnectionMongoDB:MongoDBName"));
            collection = db.GetCollection<BsonDocument>(tableName);
        }

        /// <summary>
        /// 根据管道获取数据列表
        /// </summary>
        /// <param name="stageList"></param>
        /// <returns></returns>
        public async Task<List<BsonDocument>> GetListAggregateAsync(IList<IPipelineStageDefinition> stageList)
        {
            PipelineDefinition<BsonDocument, BsonDocument> pipeline = new PipelineStagePipelineDefinition<BsonDocument, BsonDocument>(stageList);
            var result = await collection.AggregateAsync(pipeline);
            return await result.ToListAsync();
        }

        /// <summary>
        /// 根据管道获取单条数据
        /// </summary>
        /// <param name="stageList"></param>
        /// <returns></returns>
        public async Task<BsonDocument> GetAggregateAsync(IList<IPipelineStageDefinition> stageList)
        {
            PipelineDefinition<BsonDocument, BsonDocument> pipeline = new PipelineStagePipelineDefinition<BsonDocument, BsonDocument>(stageList);
            var result = await collection.AggregateAsync(pipeline);
            return await result.FirstOrDefaultAsync();
        }
    }
}
