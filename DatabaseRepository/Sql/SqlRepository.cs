using DatabaseRepository.Constants;
using MongoDB.Driver;

namespace DatabaseRespository.Sql
{
    public class SqlRepository<I> : ISqlRepository<I>
    {
        public Task Add(HashSet<I> items)
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

        public Task Delete(HashSet<string> ids)
        {
            throw new NotImplementedException();
        }

        public Task Delete(FilterDefinition<I> filterDefinition)
        {
            throw new NotImplementedException();
        }

        public Task Deactivate(HashSet<string> ids, bool isDeleted)
        {
            throw new NotImplementedException();
        }

        public Task<(HashSet<I>, long)> GetAll(bool isDeleted = false, int? skip = 0, int? limit = 0, string? sortBy = BaseConstant.CreationDateTime, bool ascending = false)
        {
            throw new NotImplementedException();
        }

        public Task<(HashSet<I>, long)> GetByField(FilterDefinition<I> filterDefinition, bool isDeleted = false, int? skip = 0, int? limit = 0, string? sortBy = BaseConstant.CreationDateTime, bool ascending = false)
        {
            throw new NotImplementedException();
        }

        public Task<(HashSet<I>, long)> GetById(HashSet<string> ids, bool isDeleted = false, int? skip = 0, int? limit = 0, string? sortBy = BaseConstant.CreationDateTime, bool ascending = false)
        {
            throw new NotImplementedException();
        }

        public SqlRepository<I> GetClient()
        {
            throw new NotImplementedException();
        }

        public Task Update(FilterDefinition<I> filterDefinition, UpdateDefinition<I> updateDefinition, bool upsert)
        {
            throw new NotImplementedException();
        }
    }
}
