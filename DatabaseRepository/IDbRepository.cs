using DatabaseRepository.Model;
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
        public Task Deactivate(HashSet<string> ids, bool isDeleted = false);
        public Task<PagedResponse<I>> GetAll(Request? request = null);
        public Task<PagedResponse<I>> GetById(HashSet<string> ids, Request? request = null);
    }
}
