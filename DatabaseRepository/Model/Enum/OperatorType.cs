using System.Runtime.Serialization;

namespace DatabaseRepository.Model.Enum
{
    public enum OperatorType
    {
        [EnumMember(Value = "Equal")]
        Equal,
        [EnumMember(Value = "AnyEq")]
        AnyEq,
        [EnumMember(Value = "In")]
        In,
        [EnumMember(Value = "Like")]
        Like,
        [EnumMember(Value = "StartsWith")]
        StartsWith,
        [EnumMember(Value = "EndsWith")]
        EndsWith
    }
}
