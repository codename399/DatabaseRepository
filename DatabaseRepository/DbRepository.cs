using DatabaseRepository.Model;
using MongoDB.Driver;

namespace DatabaseRespository
{
    public class DbRepository<T, I> : IDbRepository<T, I>
    {
        public async Task Add(HashSet<I> items)
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

        public async Task Delete(HashSet<string> ids)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(FilterDefinition<I> filterDefinition)
        {
            throw new NotImplementedException();
        }

        public async Task Deactivate(HashSet<string> ids, bool isDeleted = false)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResponse<I>> GetAll(Request? request = null)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResponse<I>> GetByField(FilterDefinition<I> filterDefinition, Request? request = null)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResponse<I>> GetById(HashSet<string> ids, Request? request = null)
        {
            throw new NotImplementedException();
        }

        public T GetClient()
        {
            throw new NotImplementedException();
        }

        public async Task Update(FilterDefinition<I> filterDefinition, UpdateDefinition<I> updateDefinition, bool upsert)
        {
            throw new NotImplementedException();
        }
    }
}
