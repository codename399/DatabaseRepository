using MathNet.Numerics.RootFinding;
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
        public HashSet<I> GetAll();
        public HashSet<I> GetById(HashSet<string> ids);
        public HashSet<I> GetByField(FilterDefinition<I> filterDefinition);
    }
}
