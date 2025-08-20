using DatabaseRepository.Constants;
using DatabaseRepository.Model;
using DatabaseRepository.Model.Enum;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DatabaseRespository.MongoDb
{
    public class MongoDbRepository<I> : IMongoDbRepository<I>
    {
        private MongoClient _mongoClient;
        private IMongoDatabase _database;
        private IMongoCollection<I> _mongoCollection;

        public MongoDbRepository(string connectionString, string databaseName, string tableName, CollectionOption? collectioOption = null)
        {
            CreateClient(connectionString, databaseName);
            if (collectioOption != null)
            {
                CreateTableAndIndexes(tableName, collectioOption);
            }
            else
            {
                CreateTableAndIndexes(tableName);
            }
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

        public async Task CreateTableAndIndexes(string tableName, CollectionOption? collectionOption)
        {
            CreateTableAndIndexes(tableName);

            if (_mongoCollection != null)
            {
                if (collectionOption != null)
                {
                    if (collectionOption.UniqueColumns is { Count: > 0 })
                    {
                        var indexes = _mongoCollection.Indexes.List();
                        var indexList = await indexes.ToListAsync();
                        if (!indexList.Any(index => index["name"] == collectionOption.IndexName))
                        {
                            CreateIndex(collectionOption);
                        }
                    }
                }
            }
        }

        private async Task CreateIndex(CollectionOption? collectionOption)
        {
            IndexKeysDefinition<I> indexDefinition = null;

            foreach (string column in collectionOption.UniqueColumns)
            {
                IndexKeysDefinition<I> tempDefinition = Builders<I>.IndexKeys.Ascending(column);
                indexDefinition = indexDefinition == null ? tempDefinition : Builders<I>.IndexKeys.Combine(indexDefinition, tempDefinition);
            }

            CreateIndexOptions createIndexOptions = new CreateIndexOptions() { Unique = true, Name = collectionOption.IndexName };
            CreateIndexModel<I> createIndexModel = new CreateIndexModel<I>(indexDefinition, createIndexOptions);

            await _mongoCollection.Indexes.CreateOneAsync(createIndexModel);
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

        public async Task<PagedResponse<I>> GetById(HashSet<string> ids, Request? request = null)
        {
            var builders = Builders<I>.Filter;

            FilterDefinition<I> filterDefinition = builders.In(BaseConstant._id, ids);

            return await Get(filterDefinition, request);
        }

        private async Task<PagedResponse<I>> Get(FilterDefinition<I> filterDefinition, Request? request = null)
        {
            if (request is not null)
            {
                if (request.Filters != null)
                {
                    if (request.Filters is { Count: > 0 })
                    {
                        foreach (var filter in request.Filters)
                        {
                            if (filter.Operator == OperatorType.Equal.ToString())
                            {
                                filterDefinition = filterDefinition & Builders<I>.Filter.Eq(filter.Key, filter.Value);
                            }
                            else if (filter.Operator == OperatorType.In.ToString())
                            {
                                filterDefinition = filterDefinition & Builders<I>.Filter.In(filter.Key, filter.Value);
                            }
                            else if (filter.Operator == OperatorType.Like.ToString())
                            {
                                filterDefinition = filterDefinition & Builders<I>.Filter.Regex(filter.Key, new BsonRegularExpression(filter.Value, "i"));
                            }
                            else if (filter.Operator == OperatorType.StartsWith.ToString())
                            {
                                filterDefinition = filterDefinition & Builders<I>.Filter.Regex(filter.Key, new BsonRegularExpression($"^{filter.Value}", "i"));
                            }
                            else if (filter.Operator == OperatorType.EndsWith.ToString())
                            {
                                filterDefinition = filterDefinition & Builders<I>.Filter.Regex(filter.Key, new BsonRegularExpression($"{filter.Value}$", "i"));
                            }
                        }
                    }
                }

                if (request.IsDeleted != null)
                {
                    filterDefinition = filterDefinition & Builders<I>.Filter.Eq(BaseConstant.IsDeleted, request.IsDeleted);
                }
            }
            else
            {
                filterDefinition = filterDefinition & Builders<I>.Filter.Eq(BaseConstant.IsDeleted, false);
            }

            var query = _mongoCollection.Find(filterDefinition);
            long count = await _mongoCollection.CountDocumentsAsync(filterDefinition);

            if (request is not null)
            {
                SortDefinition<I> sort = request.Ascending ?
                   Builders<I>.Sort.Ascending(request.SortBy) :
                   Builders<I>.Sort.Descending(request.SortBy);

                query = query.Sort(sort);

                if (!request.FetchAll)
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

        public void CreateTableAndIndexes(string tableName)
        {
            _mongoCollection = _database.GetCollection<I>(tableName);

            if (_mongoCollection == null)
            {
                _database.CreateCollection(tableName);
                _mongoCollection = _database.GetCollection<I>(tableName);
            }
        }
    }
}
