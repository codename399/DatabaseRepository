namespace DatabaseRepository.Model
{
    public class PagedResponse<I>
    {
        public required HashSet<I> Items { get; set; }
        public long Count { get; set; }
    }
}
