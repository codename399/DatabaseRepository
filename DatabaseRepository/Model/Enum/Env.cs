using System.Runtime.Serialization;

namespace ChawlEventAPI.Model.Enum
{
    public enum Env
    {
        [EnumMember(Value = "Local")]
        Local,
        [EnumMember(Value = "Dev")]
        Dev
    }
}
