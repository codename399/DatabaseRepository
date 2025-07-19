using DatabaseRespository.Model.Enum;

namespace DatabaseRespository.Model.Dto
{
    public class MongoDatabaseSettingDto : MongoDatabaseSetting
    {
        public string? ConnectionString
        {
            get
            {
                if (this.Environment == nameof(Env.Dev))
                {
                    return this.DevConnectionString;
                }

                return this.LocalConnectionString;
            }
            set { ConnectionString = value; }
        }
    }
}
