﻿using MongoDB.Driver;

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

        public void Add(HashSet<I> items)
        {
            if (items is { Count: <= 0 })
            {
                return;
            }

            _mongoCollection.InsertMany(items);
        }

        public void Update(FilterDefinition<I> filterDefinition, UpdateDefinition<I> updateDefinition, bool upsert)
        {
            _mongoCollection.UpdateMany(filterDefinition, updateDefinition, new UpdateOptions { IsUpsert = upsert });
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

        public void Delete(HashSet<string> ids)
        {
            FilterDefinition<I> filterDefinition = Builders<I>.Filter.In("id", ids);

            _mongoCollection.DeleteMany(filterDefinition);
        }

        public void Delete(FilterDefinition<I> filterDefinition)
        {
            _mongoCollection.DeleteMany(filterDefinition);
        }

        public HashSet<I> GetAll()
        {
            FilterDefinition<I> filterDefinition = Builders<I>.Filter.Empty;

            return Get(filterDefinition);
        }

        public HashSet<I> GetByField(FilterDefinition<I> filterDefinition)
        {
            return Get(filterDefinition);
        }

        public HashSet<I> GetById(HashSet<string> ids)
        {
            FilterDefinition<I> filterDefinition = Builders<I>.Filter.In("Id", ids);

            return Get(filterDefinition);
        }

        private HashSet<I> Get(FilterDefinition<I> filterDefinition)
        {
            var items = _mongoCollection.Find(filterDefinition).ToList();

            return items == null ? new HashSet<I>() : items.ToHashSet();
        }

        public MongoClient GetClient()
        {
            return _mongoClient;
        }
    }
}
