using DatabaseRepository.Model;
using MongoDB.Driver;

namespace DatabaseRespository.MongoDb
{
    public interface IMongoDbRepository<I> : IDbRepository<MongoClient, I>
    {
        public Task CreateTableAndIndexes(string tableName, CollectionOption? collectionOption = null);
    }
}
