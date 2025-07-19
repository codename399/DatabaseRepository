using System.Runtime.Serialization;

namespace DatabaseRespository.Model.Enum
{
    public enum Env
    {
        [EnumMember(Value = "Local")]
        Local,
        [EnumMember(Value = "Dev")]
        Dev
    }
}
