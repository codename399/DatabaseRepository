using DatabaseRepository.Constants;
using MongoDB.Driver;

namespace DatabaseRespository
{
    public interface IDbRepository<T, I>
    {
        public void CreateClient(string connectionString, string databaseName);
        public T GetClient();
        public void CreateTableAndIndexes(string tableName);
        public Task Add(HashSet<I> items);
        public Task Update(FilterDefinition<I> filterDefinition, UpdateDefinition<I> updateDefinition, bool upsert);
        public Task Delete(HashSet<string> ids);
        public Task Delete(FilterDefinition<I> filterDefinition);
        public Task Deactivate(HashSet<string> ids, bool isDeleted);
        public Task<(HashSet<I>, long)> GetAll(bool isDeleted = false, int? skip = 0, int? limit = 0, string? sortBy = BaseConstant.CreationDateTime, bool ascending = false);
        public Task<(HashSet<I>, long)> GetById(HashSet<string> ids, bool isDeleted = false, int? skip = 0, int? limit = 0, string? sortBy = BaseConstant.CreationDateTime, bool ascending = false);
        public Task<(HashSet<I>, long)> GetByField(FilterDefinition<I> filterDefinition, bool isDeleted = false, int? skip = 0, int? limit = 0, string? sortBy = BaseConstant.CreationDateTime, bool ascending = false);
    }
}
