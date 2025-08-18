namespace DatabaseRepository.Model
{
    public class FilterRequest
    {
        public required string Key { get; set; }
        public required string Value { get; set; }
        public required string Operator { get; set; }
    }
}
