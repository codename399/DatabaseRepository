namespace DatabaseRepository.Model
{
    public class CollectionOption
    {
        public string IndexName { get; set; }
        public HashSet<string> UniqueColumns { get; set; }
    }
}
