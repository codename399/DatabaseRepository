namespace DatabaseRepository.Model
{
    public class FilterRequest
    {
        public required string Key { get; set; }
        public string? Value { get; set; }
        public HashSet<string>? Values { get; set; }
        public required string Operator { get; set; }
    }
}
