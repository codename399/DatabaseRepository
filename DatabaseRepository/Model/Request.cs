using DatabaseRepository.Constants;

namespace DatabaseRepository.Model
{
    public class Request
    {
        public int? Skip { get; set; }
        public int? Limit { get; set; }
        public string? SortBy { get; set; }
        public bool Ascending { get; set; }
        public bool? IsDeleted { get; set; }
        public Dictionary<string,string>? Filters { get; set; }
        public bool FetchAll { get; set; }

        public Request()
        {
            Skip = 0;
            Limit = 5;
            SortBy = BaseConstant.CreationDateTime;
            Ascending = false;
            IsDeleted = false;
            FetchAll = false;
        }
    }
}
