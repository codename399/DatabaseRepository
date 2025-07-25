﻿using DatabaseRespository.Model;
using DatabaseRespository.Model.Dto;
using DatabaseRespository.MongoDb;
using Microsoft.Extensions.Options;

namespace DatabaseRespository.Repositories.Abstractions
{
    public abstract class GlobalRepository<T>
    {
        protected readonly IMongoDbRepository<T> _mongoDbRepository;
        private readonly MongoDatabaseSettingDto _databaseSetting;

        public GlobalRepository(IOptions<MongoDatabaseSetting> databaseSetting, string tableName)
        {
            _databaseSetting = MongoDatabaseSetting.ToDto(databaseSetting.Value);
            _mongoDbRepository = new MongoDbRepository<T>(_databaseSetting.ConnectionString, databaseSetting.Value.DatabaseName, tableName);
        }
    }
}
