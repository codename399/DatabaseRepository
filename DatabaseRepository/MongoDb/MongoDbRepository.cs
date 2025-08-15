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

        public async Task<PagedResponse<I>> GetAll(Request? request = null)
        {
            FilterDefinition<I> filterDefinition = Builders<I>.Filter.Empty;

            return await Get(filterDefinition, request);
        }

        public async Task<PagedResponse<I>> GetByField(FilterDefinition<I> filterDefinition, Request? request = null)
        {
            filterDefinition = filterDefinition & Builders<I>.Filter.Empty;

            return await Get(filterDefinition, request);
        }

        public async Task<PagedResponse<I>> GetById(HashSet<string> ids, Request? request = null)
        {
            var builders = Builders<I>.Filter;

            FilterDefinition<I> filterDefinition = builders.In(BaseConstant._id, ids);

            return await Get(filterDefinition, request);
        }

        private async Task<PagedResponse<I>> Get(FilterDefinition<I> filterDefinition, Request? request = null)
        {
            SortDefinition<I> sort = null;

            if (request is not null)
            {
                sort = request.Ascending ?
                   Builders<I>.Sort.Ascending(request.SortBy) :
                   Builders<I>.Sort.Descending(request.SortBy);
            }
            else
            {
                filterDefinition = filterDefinition & Builders<I>.Filter.Eq(BaseConstant.IsDeleted, false);
            }

            var count = await _mongoCollection.CountDocumentsAsync(filterDefinition);
            var query = _mongoCollection.Find(filterDefinition);

            if (sort is not null)
            {
                query = query.Sort(sort);
            }

            if (request is not null)
            {
                if (request.Skip.HasValue && request.Skip.Value > 0)
                {
                    query = query.Skip(request.Skip.Value);
                }

                if (request.Limit.HasValue && request.Limit.Value > 0)
                {
                    query = query.Limit(request.Limit.Value);
                }
            }

            var items = await query.ToListAsync();

            PagedResponse<I> pagedResponse = new PagedResponse<I>
            {
                Items = items.ToHashSet(),
                Count = count
            };

            return pagedResponse;
        }

        public MongoClient GetClient()
        {
            return _mongoClient;
        }

        public async Task Deactivate(HashSet<string> ids, bool isDeleted = false)
        {
            FilterDefinition<I> filter = Builders<I>.Filter.In(BaseConstant._id, ids);

            UpdateDefinition<I> update = Builders<I>.Update.Set(BaseConstant.IsDeleted, isDeleted);

            await _mongoCollection.UpdateManyAsync(filter, update);
        }
    }
}
