namespace DatabaseRepository.Model
{
    public class IndexOption
    {
        public string Name { get; set; }
        public HashSet<string> Columns { get; set; }
        public bool IsCombined { get; set; } = false;
        public bool Ascending { get; set; } = true;
    }
}
