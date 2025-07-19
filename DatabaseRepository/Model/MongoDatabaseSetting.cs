using ChawlEventAPI.Model.Dto;

namespace ChawlEvent.Model
{
    public class MongoDatabaseSetting
    {
        public string? LocalConnectionString { get; set; }
        public string? DevConnectionString { get; set; }
        public string? Environment { get; set; }
        public string? DatabaseName { get; set; }

        public static MongoDatabaseSettingDto ToDto(MongoDatabaseSetting databaseSetting)
        {
            return new MongoDatabaseSettingDto()
            {
                LocalConnectionString = databaseSetting.LocalConnectionString,
                DevConnectionString = databaseSetting.DevConnectionString,
                Environment = databaseSetting.Environment,
                DatabaseName = databaseSetting.DatabaseName
            };
        }
    }
}
