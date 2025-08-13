using DatabaseRepository.Constants;
using DatabaseRepository.Model;
using MongoDB.Driver;

namespace DatabaseRespository.MongoDb
{
    public class MongoDbRepository<I> : IMongoDbRepository<I>
    {
        private MongoClient _mongoClient;
        private IMongoDatabase _database;
        private IMongoCollection<I> _mongoCollection;

        public MongoDbRepository(string connectionString, string databaseName, string tableName)
        {
            CreateClient(connectionString, databaseName);
            CreateTableAndIndexes(tableName);
        }

        public async Task Add(HashSet<I> items)
        {
            if (items is { Count: <= 0 })
            {
                return;
            }

            await _mongoCollection.InsertManyAsync(items);
        }

        public async Task Update(FilterDefinition<I> filterDefinition, UpdateDefinition<I> updateDefinition, bool upsert)
        {
            await _mongoCollection.UpdateManyAsync(filterDefinition, updateDefinition, new UpdateOptions { IsUpsert = upsert });
        }

        public void CreateClient(string connectionString, string databaseName)
        {
            _mongoClient = new MongoClient(connectionString);
            _database = _mongoClient.GetDatabase(databaseName);
        }

        public void CreateTableAndIndexes(string tableName)
        {
            _mongoCollection = _database.GetCollection<I>(tableName);

            if (_mongoCollection == null)
            {
                _database.CreateCollection(tableName);
                _mongoCollection = _database.GetCollection<I>(tableName);
            }
        }

        public async Task Delete(HashSet<string> ids)
        {
            FilterDefinition<I> filterDefinition = Builders<I>.Filter.In(BaseConstant._id, ids);

            await _mongoCollection.DeleteManyAsync(filterDefinition);
        }

        public async Task Delete(FilterDefinition<I> filterDefinition)
        {
            await _mongoCollection.DeleteManyAsync(filterDefinition);
        }

        public Task<(HashSet<I>, long)> GetAll(Request request)
        {
            FilterDefinition<I> filterDefinition = Builders<I>.Filter.Eq(BaseConstant.IsDeleted, request.IsDeleted);

            return Get(filterDefinition, request);
        }

        public Task<(HashSet<I>, long)> GetByField(FilterDefinition<I> filterDefinition, Request request)
        {
            filterDefinition = filterDefinition & Builders<I>.Filter.Eq(BaseConstant.IsDeleted, request.IsDeleted);

            return Get(filterDefinition, request);
        }

        public Task<(HashSet<I>, long)> GetById(HashSet<string> ids, Request request)
        {
            var builders = Builders<I>.Filter;

            FilterDefinition<I> filterDefinition = builders.In(BaseConstant._id, ids)
                & builders.Eq(BaseConstant.IsDeleted, request.IsDeleted);


            return Get(filterDefinition, request);
        }

        private async Task<(HashSet<I>, long)> Get(FilterDefinition<I> filterDefinition, Request request)
        {
            SortDefinition<I> sort = request.Ascending ?
                Builders<I>.Sort.Ascending(request.SortBy) :
                Builders<I>.Sort.Descending(request.SortBy);

            var count = await _mongoCollection.CountDocumentsAsync(filterDefinition);
            var query = _mongoCollection.Find(filterDefinition).Sort(sort);

            if (request.Skip.HasValue && request.Skip.Value > 0)
            {
                query = query.Skip(request.Skip.Value);
            }

            if (request.Limit.HasValue && request.Limit.Value > 0)
            {
                query = query.Limit(request.Limit.Value);
            }

            var items = await query.ToListAsync();

            return (items == null ? new HashSet<I>() : items.ToHashSet(), count);
        }

        public MongoClient GetClient()
        {
            return _mongoClient;
        }

        public async Task Deactivate(HashSet<string> ids, bool isDeleted)
        {
            FilterDefinition<I> filter = Builders<I>.Filter.In(BaseConstant._id, ids);

            UpdateDefinition<I> update = Builders<I>.Update.Set(BaseConstant.IsDeleted, isDeleted);

            await _mongoCollection.UpdateManyAsync(filter, update);
        }
    }
}
