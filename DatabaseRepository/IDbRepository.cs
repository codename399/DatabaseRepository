using MongoDB.Driver;

namespace DatabaseRespository
{
    public interface IDbRepository<T, I>
    {
        public void CreateClient(string connectionString, string databaseName);
        public T GetClient();
        public void CreateTableAndIndexes(string tableName);
        public void Add(HashSet<I> items);
        public void Update(FilterDefinition<I> filterDefinition, UpdateDefinition<I> updateDefinition, bool upsert);
        public void Delete(HashSet<string> ids);
        public void Delete(FilterDefinition<I> filterDefinition);
        public void Delete(HashSet<string> ids, bool isDeleted);
        public HashSet<I> GetAll(bool isDeleted = false);
        public HashSet<I> GetById(HashSet<string> ids, bool isDeleted = false);
        public HashSet<I> GetByField(FilterDefinition<I> filterDefinition, bool isDeleted = false);
    }
}
