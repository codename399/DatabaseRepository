using DatabaseRepository.Model;
using DatabaseRespository.Model;
using DatabaseRespository.Model.Dto;
using DatabaseRespository.MongoDb;

namespace DatabaseRespository.Repositories.Abstractions
{
    public abstract class GlobalRepository<T>
    {
        protected readonly IMongoDbRepository<T> _mongoDbRepository;
        private readonly MongoDatabaseSettingDto _databaseSetting;

        public GlobalRepository(MongoDatabaseSetting databaseSetting, string tableName, CollectionOption? collectionOption = null)
        {
            _databaseSetting = MongoDatabaseSetting.ToDto(databaseSetting);
            _mongoDbRepository = new MongoDbRepository<T>(_databaseSetting.ConnectionString ?? string.Empty, databaseSetting.DatabaseName ?? string.Empty, tableName, collectionOption);
        }
    }
}
