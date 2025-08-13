using DatabaseRepository.Constants;
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

        public Task<(HashSet<I>, long)> GetAll(bool isDeleted = false, int? skip = 0, int? limit = 0, string? sortBy = BaseConstant.CreationDateTime, bool ascending = false)
        {
            FilterDefinition<I> filterDefinition = Builders<I>.Filter.Eq(BaseConstant.IsDeleted, isDeleted);

            return Get(filterDefinition, skip, limit, sortBy, ascending);
        }

        public Task<(HashSet<I>, long)> GetByField(FilterDefinition<I> filterDefinition, bool isDeleted = false, int? skip = 0, int? limit = 0, string? sortBy = BaseConstant.CreationDateTime, bool ascending = false)
        {
            filterDefinition = filterDefinition & Builders<I>.Filter.Eq(BaseConstant.IsDeleted, isDeleted);

            return Get(filterDefinition, skip, limit, sortBy, ascending);
        }

        public Task<(HashSet<I>, long)> GetById(HashSet<string> ids, bool isDeleted = false, int? skip = 0, int? limit = 0, string? sortBy = BaseConstant.CreationDateTime, bool ascending = false)
        {
            var builders = Builders<I>.Filter;

            FilterDefinition<I> filterDefinition = builders.In(BaseConstant._id, ids)
                & builders.Eq(BaseConstant.IsDeleted, isDeleted);


            return Get(filterDefinition, skip, limit, sortBy, ascending);
        }

        private async Task<(HashSet<I>, long)> Get(FilterDefinition<I> filterDefinition, int? skip = 0, int? limit = 0, string? sortBy = BaseConstant.CreationDateTime, bool ascending = false)
        {
            SortDefinition<I> sort = ascending ?
                Builders<I>.Sort.Ascending(sortBy) :
                Builders<I>.Sort.Descending(sortBy);

            var count = await _mongoCollection.CountDocumentsAsync(filterDefinition);
            var query = _mongoCollection.Find(filterDefinition).Sort(sort);

            if (skip.HasValue && skip.Value > 0)
            {
                query = query.Skip(skip.Value);
            }

            if (limit.HasValue && limit.Value > 0)
            {
                query = query.Limit(limit.Value);
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
