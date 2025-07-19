using ChawlEvent.Model;
using ChawlEventAPI.Model.Enum;

namespace ChawlEventAPI.Model.Dto
{
    public class MongoDatabaseSettingDto:MongoDatabaseSetting
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
