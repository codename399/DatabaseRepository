﻿using MongoDB.Driver;

namespace DatabaseRespository.Sql
{
    public class SqlRepository<I> : ISqlRepository<I>
    {
        public void Add(HashSet<I> items)
        {
            throw new NotImplementedException();
        }

        public void CreateClient(string connectionString, string databaseName)
        {
            throw new NotImplementedException();
        }

        public void CreateTableAndIndexes(string tableName)
        {
            throw new NotImplementedException();
        }

        public void Delete(HashSet<string> ids)
        {
            throw new NotImplementedException();
        }

        public void Delete(FilterDefinition<I> filterDefinition)
        {
            throw new NotImplementedException();
        }

        public HashSet<I> GetAll()
        {
            throw new NotImplementedException();
        }

        public HashSet<I> GetByField(FilterDefinition<I> filterDefinition)
        {
            throw new NotImplementedException();
        }

        public HashSet<I> GetById(HashSet<string> ids)
        {
            throw new NotImplementedException();
        }

        public SqlRepository<I> GetClient()
        {
            throw new NotImplementedException();
        }

        public void Update(FilterDefinition<I> filterDefinition, UpdateDefinition<I> updateDefinition, bool upsert)
        {
            throw new NotImplementedException();
        }
    }
}
