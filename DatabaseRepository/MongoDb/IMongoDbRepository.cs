using MongoDB.Driver;

namespace DatabaseRespository.MongoDb
{
    public interface IMongoDbRepository<I> : IDbRepository<MongoClient, I>
    {
    }
}
